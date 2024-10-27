using System;
using SimbirHealth.Timetable.Models.Requests;
using SimbirHealth.Timetable.Models.Responses;

namespace SimbirHealth.Timetable.Services.TimetableService;

public interface ITimetableService
{
    Task<IResult> PostTimetable(AddOrUpdateTimetableRequest request, string accessToken);
    Task<IResult> PutTimetable(Guid guid, AddOrUpdateTimetableRequest request, string accessToken);
    Task<IResult> SoftDeleteTimetable(Guid guid);
    Task<IResult> SoftDeleteTimetableByDoctor(Guid doctorGuid, string accessToken);
    Task<IResult> SoftDeleteTimetableByHospital(Guid hospitalGuid, string accessToken);
    Task<IResult> GetTimetablesByHospital(Guid hospitalGuid,
        DateTime from, DateTime to, string accessToken);
    Task<IResult> GetTimetablesByDoctor(Guid doctorGuid,
        DateTime from, DateTime to, string accessToken);
    Task<IResult> GetTimetablesByRoom(Guid hospitalGuid, 
        string roomName, DateTime from, DateTime to, string accessToken);
    Task<IResult> GetAppointments(Guid timetableGuid);
    Task<IResult> PostTimetable(Guid id, DateTime time, string accessToken);
}
