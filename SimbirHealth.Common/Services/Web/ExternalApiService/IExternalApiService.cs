using Microsoft.IdentityModel.Tokens;
using SimbirHealth.Data.Models.Account;
using SimbirHealth.Data.SharedResponses.Account;
using SimbirHealth.Data.SharedResponses.Hospital;

namespace SimbirHealth.Common.Services.Web.ExternalApiService;

public interface IExternalApiService
{
    Task<DoctorResponse?> GetDoctorByGuid(Guid doctorGuid, string accessToken);
    Task<HospitalResponse?> GetHospitalByGuid(Guid hospitalGuid, string accessToken);
    Task<List<RoomResponse>?> GetHospitalRoomsByGuid(Guid hospitalGuid, string accessToken);
    Task<IDictionary<string, object>?> ValidateToken(string accessToken);
    Task<AcccountResponse?> GetAccountByGuid(Guid accountGuid, string accessToken);
}
