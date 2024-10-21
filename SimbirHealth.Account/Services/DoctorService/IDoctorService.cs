using SimbirHealth.Account.Models.Requests.Doctor;
using SimbirHealth.Data.SharedResponses.Account;

namespace SimbirHealth.Account.Services.DoctorService
{
    public interface IDoctorService
    {
        Task<List<DoctorResponse>?> AllDoctors(AllDoctorsRequest request);
        Task<DoctorResponse?> Doctor(Guid guid);
    }
}