using Microsoft.EntityFrameworkCore;
using SimbirHealth.Common.Services.Db.Repositories;
using SimbirHealth.Data.Models.Timetable;
using SimbirHealth.Timetable.Models.Requests;
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



        public async Task<IResult> PostTimetable(AddOrUpdateTimetableRequest request, string accessToken){
            var doctor = await _externalApiService.GetDoctorByGuid(request.DoctorId, accessToken);
            var hospital = await _externalApiService.GetHospitalByGuid(request.HospitalId, accessToken);

            bool canMakeTimetable = doctor != null && 
                hospital != null && 
                hospital.Rooms.Select(r => r.RoomName).Contains(request.Room) &&
                TimeHelperService.ValidateInterval(request.From, request.To);
            
            if (canMakeTimetable){
                var room = hospital!.Rooms.Where(r => r.RoomName == request.Room).FirstOrDefault();
                if (room != null){
                    if (!await IsRoomOrDoctorBusy(room.RoomGuid, doctor!.Guid, request.From, request.To)){
                        var timetable = _timetableRepository.Add(new(){
                            From = request.From,
                            To = request.To,
                            RoomGuid = room.RoomGuid,
                            DoctorGuid = doctor!.Guid,
                            HospitalGuid = hospital.Guid,
                        });

                        foreach( var ticketTime in TimeHelperService.SplitDateRange(request.From, request.To)){
                            _appointmentsRepository.Add(new(){
                                Time = ticketTime,
                                Timetable = timetable
                            });
                        }

                        await _timetableRepository.SaveChangesAsync();
                        return Results.Ok("Расписание создано");
                    }
                    else{
                        return Results.BadRequest("Врач или кабинет уже имеют расписание на это время");
                    }
                }
            }
            return Results.BadRequest("Невозможно создать расписание, ошибка во входных данных");
        }


        private async Task<bool> IsRoomOrDoctorBusy(Guid roomGuid, Guid doctorGuid, DateTime rFrom, DateTime rTo){
            return await _timetableRepository
                .Query()
                .Where(t => (t.RoomGuid == roomGuid || t.DoctorGuid == doctorGuid) &&
                    ((rTo > t.From && rTo <= t.To) ||
                    (rFrom >= t.From && rFrom < t.To)))
                .AnyAsync();
        }
    }
}