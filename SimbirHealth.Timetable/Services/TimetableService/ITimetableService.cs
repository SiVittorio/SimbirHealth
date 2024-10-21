using System;
using SimbirHealth.Timetable.Models.Requests;

namespace SimbirHealth.Timetable.Services.TimetableService;

public interface ITimetableService
{
    Task<IResult> PostTimetable(AddOrUpdateTimetableRequest request, string accessToken);
}
