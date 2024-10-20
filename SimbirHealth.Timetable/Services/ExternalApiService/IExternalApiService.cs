using System;

namespace SimbirHealth.Timetable.Services.ExternalApiService;

public interface IExternalApiService
{
    Task<string?> GetDoctorByGuid(Guid doctorGuid, string accessToken);
}
