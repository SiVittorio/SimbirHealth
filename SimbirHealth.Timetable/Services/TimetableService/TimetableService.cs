using System.Net;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using SimbirHealth.Common.Migrations;
using SimbirHealth.Common.Services.Db.Repositories;
using SimbirHealth.Common.Services.Web.ExternalApiService;
using SimbirHealth.Data.Models.Hospital;
using SimbirHealth.Data.Models.Timetable;
using SimbirHealth.Data.SharedResponses.Account;
using SimbirHealth.Data.SharedResponses.Hospital;
using SimbirHealth.Timetable.Models.Requests;
using SimbirHealth.Timetable.Models.Responses;
using SimbirHealth.Timetable.Services.TimeValidator;

namespace SimbirHealth.Timetable.Services.TimetableService
{
    public class TimetableService : ITimetableService 
    {
        private readonly IRepositoryBase<TimetableModel> _timetableRepository;
        private readonly IRepositoryBase<Appointment> _appointmentsRepository;
        private readonly IExternalApiService _externalApiService;


        private const string doctorNotValid = "Неверный идентификатор врача";
        private const string timetableNotValid = "Неверный идентификатор расписания";
        private const string hospitalNotValid = "Неверный идентификатор больницы";

        public TimetableService(IRepositoryBase<TimetableModel> timetableRepository,
        IRepositoryBase<Appointment> appointmentsRepository,
        IExternalApiService externalApiService){
            _timetableRepository = timetableRepository;
            _appointmentsRepository = appointmentsRepository;
            _externalApiService = externalApiService;
        }

        /// <summary>
        /// Создать новое расписание
        /// </summary>
        /// <param name="request">Запрос на создание</param>
        /// <param name="accessToken">JWT токен</param>
        /// <returns></returns>
        public async Task<IResult> PostTimetable(AddOrUpdateTimetableRequest request, string accessToken){
            var doctor = await _externalApiService.GetDoctorByGuid(request.DoctorId, accessToken);
            var hospital = await _externalApiService.GetHospitalByGuid(request.HospitalId, accessToken);
            var rooms = await _externalApiService.GetHospitalRoomsByGuid(request.HospitalId, accessToken);
            
            var validationResult = await FullTimetableValidation(request, doctor, hospital, rooms);
            if (validationResult.result != Results.Ok())
                return validationResult.result;
            
            RoomResponse room = validationResult.room!;

            var timetable = _timetableRepository.Add(new(){
                From = request.From,
                To = request.To,
                RoomGuid = room.RoomGuid,
                DoctorGuid = doctor!.Guid,
                HospitalGuid = hospital!.Guid,
            });

            foreach( var ticketTime in TimeHelperService.SplitDateRange(request.From, request.To)){
                _appointmentsRepository.Add(new(){
                    Time = ticketTime,
                    Timetable = timetable
                });
            }

            await _timetableRepository.SaveChangesAsync();
            return Results.Created();
        }


        /// <summary>
        /// Изменить существующее расписание
        /// </summary>
        /// <param name="guid">Guid расписания</param>
        /// <param name="request">Запрос на изменение</param>
        /// <param name="accessToken">JWT токен</param>
        /// <returns></returns>
        public async Task<IResult> PutTimetable(Guid guid, AddOrUpdateTimetableRequest request, string accessToken){
            var timetable = await _timetableRepository.Query()
                .Include(t => t.Appointments)
                .FirstOrDefaultAsync(t => t.Guid == guid);
            
            if (timetable == null) return Results.BadRequest(timetableNotValid);
            if (timetable.Appointments.Any(a => a.IsTaken && !a.IsDeleted)) 
                return Results.BadRequest("Нельзя редактировать, если есть записавшиеся на прием");

            var doctor = await _externalApiService.GetDoctorByGuid(request.DoctorId, accessToken);
            var hospital = await _externalApiService.GetHospitalByGuid(request.HospitalId, accessToken);
            var rooms = await _externalApiService.GetHospitalRoomsByGuid(request.HospitalId, accessToken);
            
            var validationResult = await FullTimetableValidation(request, doctor, hospital, rooms, guid);

            if (validationResult.result != Results.Ok())
                return validationResult.result;
            
            RoomResponse room = validationResult.room!;

            _appointmentsRepository.DeleteRange(
                await _appointmentsRepository
                    .Query()
                    .Where(a => a.TimetableGuid == guid)
                    .ToListAsync()
            );

            await _appointmentsRepository.SaveChangesAsync();

            timetable.From = request.From;
            timetable.To = request.To;
            timetable.HospitalGuid = request.HospitalId;
            timetable.DoctorGuid = request.DoctorId;
            timetable.RoomGuid = room.RoomGuid;
            timetable.Appointments = new();

            foreach( var ticketTime in TimeHelperService.SplitDateRange(request.From, request.To)){
                _appointmentsRepository.Add(new(){
                    Time = ticketTime,
                    Timetable = timetable
                });
            }

            await _timetableRepository.SaveChangesAsync();
            return Results.Ok("Расписание обновлено");
        }


        public async Task<IResult> SoftDeleteTimetable(Guid guid){
            var timetable = await _timetableRepository.Query()
                .Include(t => t.Appointments)
                .FirstOrDefaultAsync(t => t.Guid == guid);
            
            if (timetable == null) return Results.BadRequest(timetableNotValid);

            timetable.IsDeleted = true;
            timetable.Appointments.ForEach(a => a.IsDeleted = true);

            _timetableRepository.Update(timetable);
            await _timetableRepository.SaveChangesAsync();

            return Results.Ok();
        }

        public async Task<IResult> SoftDeleteTimetableByDoctor(Guid doctorGuid, string accessToken){
            
            if (await _externalApiService.GetDoctorByGuid(doctorGuid, accessToken) == null)
                return Results.BadRequest(doctorNotValid);
            var timetables = await _timetableRepository
                .Query()
                .Include(t => t.Appointments)
                .Where(t => t.DoctorGuid == doctorGuid)
                .ToListAsync();
            
            timetables.ForEach(t =>{
                t.IsDeleted = true;
                t.Appointments.ForEach(
                    a =>a.IsDeleted = true
                );
            });

            _timetableRepository.UpdateRange(timetables);
            await _timetableRepository.SaveChangesAsync();

            return Results.Ok();
        }

        public async Task<IResult> SoftDeleteTimetableByHospital(Guid hospitalGuid, string accessToken)
        {
            if (await _externalApiService.GetHospitalByGuid(hospitalGuid, accessToken) == null)
                return Results.BadRequest(hospitalNotValid);
            var timetables = await _timetableRepository
                .Query()
                .Include(t => t.Appointments)
                .Where(t => t.HospitalGuid == hospitalGuid)
                .ToListAsync();

            timetables.ForEach(t =>{
                t.IsDeleted = true;
                t.Appointments.ForEach(
                    a =>a.IsDeleted = true
                );
            });
            
            await _timetableRepository.SaveChangesAsync();

            return Results.Ok();
        }

        public async Task<IResult> GetTimetablesByHospital(Guid hospitalGuid,
            DateTime from, DateTime to, string accessToken)
        {
            var hospital = await _externalApiService.GetHospitalByGuid(hospitalGuid, accessToken);

            if (hospital == null)
                return Results.NotFound();
            
            var timetables = await _timetableRepository
                .Query()
                .Include(t => t.Appointments)
                .Where(t => t.From >= from && t.To <= to && t.HospitalGuid == hospital.Guid)
                .OrderBy(t => t.From)
                .ToListAsync();

            var doctors = new Dictionary<Guid, DoctorResponse>();
            var rooms = new Dictionary<Guid, List<RoomResponse>>();
            var response = new List<GetTimetableByHospitalResponse>();

            foreach(var tTable in timetables){
                (DoctorResponse? doctor, List<RoomResponse>? rooms) data;
                if (!doctors.TryGetValue(tTable.DoctorGuid, out data.doctor))
                    data.doctor = await _externalApiService.GetDoctorByGuid(tTable.DoctorGuid, accessToken);

                if (!rooms.TryGetValue(tTable.HospitalGuid, out data.rooms))
                    data.rooms = await _externalApiService.GetHospitalRoomsByGuid(tTable.HospitalGuid, accessToken);

                if (data.doctor == null ||
                    data.rooms.IsNullOrEmpty()) return Results.NotFound();

                var room = data.rooms!.FirstOrDefault(r => r.RoomGuid == tTable.RoomGuid);
                if (room == null) return Results.NotFound();

                doctors.Add(tTable.DoctorGuid, data.doctor);
                rooms.Add(tTable.RoomGuid, data.rooms!);

                response.Add(new(
                    tTable.From,
                    tTable.To,
                    tTable.Appointments
                    .OrderBy(a => a.Time)
                    .Select(
                        a => new GetAppointmentResponse(a.Time, a.IsTaken)).ToList(),
                    room.RoomName,
                    string.Format("{0} {1}", data.doctor.LastName, data.doctor.FirstName)));

            }
            return Results.Ok(response);
        }


        public async Task<IResult> GetTimetablesByDoctor(Guid doctorGuid,
            DateTime from, DateTime to, string accessToken){
            
            var doctor = await _externalApiService.GetDoctorByGuid(doctorGuid, accessToken);

            if (doctor == null)
                return null;
            
            var timetables = await _timetableRepository
                .Query()
                .Include(t => t.Appointments)
                .Where(t => t.From >= from && t.To <= to && t.DoctorGuid == doctorGuid)
                .OrderBy(t => t.From)
                .ToListAsync();
            
            var externalValues = new Dictionary<Guid, (HospitalResponse hospital, List<RoomResponse> rooms)>();
            var response = new List<GetTimetableByDoctorResponse>();

            foreach(var tTable in timetables){
                (HospitalResponse? hospital, List<RoomResponse>? rooms) data;
                if (!externalValues.TryGetValue(tTable.HospitalGuid, out data))
                {
                    data.hospital = await _externalApiService.GetHospitalByGuid(tTable.HospitalGuid, accessToken);
                    data.rooms = await _externalApiService.GetHospitalRoomsByGuid(tTable.HospitalGuid, accessToken);
                }
                if (data.hospital == null ||
                    data.rooms.IsNullOrEmpty()) return Results.NotFound();

                var room = data.rooms!.FirstOrDefault(r => r.RoomGuid == tTable.RoomGuid);
                if (room == null) return Results.NotFound();
                externalValues.Add(tTable.HospitalGuid, data!);

                response.Add(new(
                    tTable.From,
                    tTable.To,
                    tTable.Appointments
                    .OrderBy(a => a.Time)
                    .Select(
                        a => new GetAppointmentResponse(a.Time, a.IsTaken)).ToList(),
                    data.hospital.Name,
                    room.RoomName));

            }
            return Results.Ok(response);
        }


        public async Task<IResult> GetTimetablesByRoom(Guid hospitalGuid, 
        string roomName, DateTime from, DateTime to, string accessToken)
        {
            var hospital = await _externalApiService.GetHospitalByGuid(hospitalGuid, accessToken);

            if (hospital == null)
                return Results.NotFound();

            var rooms = await _externalApiService.GetHospitalRoomsByGuid(hospitalGuid, accessToken);

            if (rooms.IsNullOrEmpty() || !rooms!.Any(r => r.RoomName == roomName))
                return Results.NotFound();
            
            var room = rooms!.First(r => r.RoomName == r.RoomName);

            var timetables = await _timetableRepository
                .Query()
                .Include(t => t.Appointments)
                .Where(t => t.From >= from && t.To <= to && t.HospitalGuid == hospital.Guid
                    && t.RoomGuid == room.RoomGuid)
                .OrderBy(t => t.From)
                .ToListAsync();

            var doctors = new Dictionary<Guid, DoctorResponse>();
            var response = new List<GetTimetableByRoomResponse>();

            foreach(var tTable in timetables){
                (DoctorResponse? doctor, List<RoomResponse>? rooms) data;
                if (!doctors.TryGetValue(tTable.DoctorGuid, out data.doctor))
                    data.doctor = await _externalApiService.GetDoctorByGuid(tTable.DoctorGuid, accessToken);

                if (data.doctor == null) return Results.NotFound();

                doctors.Add(tTable.DoctorGuid, data.doctor);

                response.Add(new(
                    tTable.From,
                    tTable.To,
                    tTable.Appointments
                    .OrderBy(a => a.Time)
                    .Select(
                        a => new GetAppointmentResponse(a.Time, a.IsTaken)).ToList(),
                    string.Format("{0} {1}", data.doctor.LastName, data.doctor.FirstName)));

            }
            return Results.Ok(response);
        }


        public async Task<IResult> GetAppointments(Guid timetableGuid){
            var appointments = await _appointmentsRepository
                .Query()
                .Where(a => a.TimetableGuid == timetableGuid && !a.IsTaken)
                .OrderBy(a => a.Time)
                .Select(a => new GetAppointmentResponse(a.Time, a.IsTaken))
                .ToListAsync();

            if (appointments.IsNullOrEmpty()) return Results.NotFound();

            return Results.Ok(appointments);
        }

        public async Task<IResult> TakeAppointment(Guid timetableGuid, DateTime time){
            var appointment = await _appointmentsRepository
                .Query()
                .Where(a => a.TimetableGuid == timetableGuid &&
                    a.Time == time)
                .FirstOrDefaultAsync();
            
            if (appointment == null) return Results.NotFound();

            appointment.IsTaken = true;
            _appointmentsRepository.Update(appointment);
            await _appointmentsRepository.SaveChangesAsync();

            return Results.Ok();
        }

        #region PRIVATE
        private async Task<(IResult result, RoomResponse? room)> FullTimetableValidation(
            AddOrUpdateTimetableRequest request, 
            DoctorResponse? doctor, 
            HospitalResponse? hospital, 
            List<RoomResponse>? rooms,
            Guid? timetablieGuid = null){
            
            if (!CanMakeTimetable(request, doctor, hospital, rooms))
                return (Results.BadRequest("Невозможно создать расписание, ошибка во входных данных"), null);

            var room = rooms.Where(r => r.RoomName == request.Room).First();

            if (await IsRoomOrDoctorBusy(room!.RoomGuid, doctor!.Guid, request.From, request.To, timetablieGuid))
                return (Results.BadRequest("Врач или кабинет уже имеют расписание на это время"), null);
            
            return (Results.Ok(), room);
        }

        private bool CanMakeTimetable(AddOrUpdateTimetableRequest request, DoctorResponse? doctor, HospitalResponse? hospital,
            List<RoomResponse>? rooms){
            return doctor != null && 
                hospital != null && 
                !rooms.IsNullOrEmpty() &&
                rooms!.Select(r => r.RoomName).Contains(request.Room) &&
                TimeHelperService.ValidateInterval(request.From, request.To);
        }
        private async Task<bool> IsRoomOrDoctorBusy(Guid roomGuid, Guid doctorGuid, DateTime rFrom, DateTime rTo,
        Guid? timetableGuid = null){
            if (timetableGuid == null){
                return await _timetableRepository
                .Query()
                .Where(t => (t.RoomGuid == roomGuid || t.DoctorGuid == doctorGuid) &&
                    ((rTo > t.From && rTo <= t.To) ||
                    (rFrom >= t.From && rFrom < t.To)) ||
                    (rFrom <= t.From && rTo >= t.To ))
                .AnyAsync();
            }
            else{
                return await _timetableRepository
                .Query()
                .Where(t => t.Guid != timetableGuid && 
                    (t.RoomGuid == roomGuid || t.DoctorGuid == doctorGuid) &&
                    ((rTo > t.From && rTo <= t.To) ||
                    (rFrom >= t.From && rFrom < t.To)) ||
                    (rFrom <= t.From && rTo >= t.To ))
                .AnyAsync();
            }
        }
        #endregion
    }
}