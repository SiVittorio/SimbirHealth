using System.Net;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using SimbirHealth.Common.Services.Db.Repositories;
using SimbirHealth.Data.Models.Timetable;
using SimbirHealth.Data.SharedResponses.Account;
using SimbirHealth.Data.SharedResponses.Hospital;
using SimbirHealth.Timetable.Models.Requests;
using SimbirHealth.Timetable.Models.Responses;
using SimbirHealth.Timetable.Services.ExternalApiService;
using SimbirHealth.Timetable.Services.TimeValidator;

namespace SimbirHealth.Timetable.Services.TimetableService
{
    public class TimetableService : ITimetableService 
    {
        private readonly IRepositoryBase<TimetableModel> _timetableRepository;
        private readonly IRepositoryBase<Appointment> _appointmentsRepository;
        private readonly IExternalApiService _externalApiService;

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

            var validationResult = await FullTimetableValidation(request, doctor, hospital);
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
            
            if (timetable == null) return Results.BadRequest("Неверный идентификатор расписания");
            if (timetable.Appointments.Any(a => a.IsTaken && !a.IsDeleted)) 
                return Results.BadRequest("Нельзя редактировать, если есть записавшиеся на прием");

            var doctor = await _externalApiService.GetDoctorByGuid(request.DoctorId, accessToken);
            var hospital = await _externalApiService.GetHospitalByGuid(request.HospitalId, accessToken);
            
            var validationResult = await FullTimetableValidation(request, doctor, hospital, guid);

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
            
            if (timetable == null) return Results.BadRequest("Неверный идентификатор расписания");

            timetable.IsDeleted = true;
            timetable.Appointments.ForEach(a => a.IsDeleted = true);

            _timetableRepository.Update(timetable);
            await _timetableRepository.SaveChangesAsync();

            return Results.Ok();
        }

        public async Task<IResult> SoftDeleteTimetableByDoctor(Guid doctorGuid){
            var timetables = await _timetableRepository
                .Query()
                .Include(t => t.Appointments)
                .Where(t => t.DoctorGuid == doctorGuid)
                .ToListAsync();
            if (timetables.IsNullOrEmpty())
                return Results.BadRequest("Неверный идентификатор врача");
            
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

        public async Task<IResult> SoftDeleteTimetableByHospital(Guid hospitalGuid)
        {
            var timetables = await _timetableRepository
                .Query()
                .Include(t => t.Appointments)
                .Where(t => t.HospitalGuid == hospitalGuid)
                .ToListAsync();
            if (timetables.IsNullOrEmpty())
                return Results.BadRequest("Неверный идентификатор больницы");

            timetables.ForEach(t =>{
                t.IsDeleted = true;
                t.Appointments.ForEach(
                    a =>a.IsDeleted = true
                );
            });
            
            await _timetableRepository.SaveChangesAsync();

            return Results.Ok();
        }

        public async Task<List<GetTimetableResponse>?> GetTimetablesByHospital(Guid hospitalGuid,
            DateTime from, DateTime to, string accessToken)
        {
            var hospital = await _externalApiService.GetHospitalByGuid(hospitalGuid, accessToken);

            if (hospital == null)
                return null;
            
            return await _timetableRepository
                .Query()
                .Include(t => t.Appointments)
                .Where(t => t.From >= from && t.To <= to)
                .OrderBy(t => t.From)
                .Select(t => new GetTimetableResponse(t.From, t.To, 
                    t.Appointments
                    .OrderBy(a => a.Time)
                    .Select(a => new GetAppointmentResponse(a.Time, a.IsTaken))
                    .ToList()))
                .ToListAsync();
        }

        private async Task<(IResult result, RoomResponse? room)> FullTimetableValidation(
            AddOrUpdateTimetableRequest request, 
            DoctorResponse? doctor, 
            HospitalResponse? hospital, Guid? timetablieGuid = null){
            
            if (!CanMakeTimetable(request, doctor, hospital))
                return (Results.BadRequest("Невозможно создать расписание, ошибка во входных данных"), null);

            var room = hospital!.Rooms.Where(r => r.RoomName == request.Room).FirstOrDefault();

            if (room == null) Results.BadRequest("Невозможно создать расписание, ошибка во входных данных");

            if (await IsRoomOrDoctorBusy(room!.RoomGuid, doctor!.Guid, request.From, request.To, timetablieGuid))
                return (Results.BadRequest("Врач или кабинет уже имеют расписание на это время"), null);
            
            return (Results.Ok(), room);
        }

        private bool CanMakeTimetable(AddOrUpdateTimetableRequest request, DoctorResponse? doctor, HospitalResponse? hospital){
            return doctor != null && 
                hospital != null && 
                hospital.Rooms.Select(r => r.RoomName).Contains(request.Room) &&
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
    }
}