using System;
using SimbirHealth.Timetable.Models.Requests;
using SimbirHealth.Timetable.Models.Responses;

namespace SimbirHealth.Timetable.Services.TimetableService;

public interface ITimetableService
{
    Task<IResult> PostTimetable(AddOrUpdateTimetableRequest request, string accessToken);
    Task<IResult> PutTimetable(Guid guid, AddOrUpdateTimetableRequest request, string accessToken);
    Task<IResult> SoftDeleteTimetable(Guid guid);
    Task<IResult> SoftDeleteTimetableByDoctor(Guid doctorGuid);
    Task<IResult> SoftDeleteTimetableByHospital(Guid hospitalGuid);
    Task<List<GetTimetableResponse>?> GetTimetablesByHospital(Guid hospitalGuid,
            DateTime from, DateTime to, string accessToken);
}
