using SimbirHealth.Data.SharedResponses.Account;
using SimbirHealth.Data.SharedResponses.Hospital;

namespace SimbirHealth.Timetable.Services.ExternalApiService;

public interface IExternalApiService
{
    Task<DoctorResponse?> GetDoctorByGuid(Guid doctorGuid, string accessToken);
    Task<HospitalResponse?> GetHospitalByGuid(Guid hospitalGuid, string accessToken);
}
