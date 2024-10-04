using SimbirHealth.Account.Models.Requests.Doctor;
using SimbirHealth.Account.Models.Responses.Doctor;

namespace SimbirHealth.Account.Services.DoctorService
{
    public interface IDoctorService
    {
        Task<List<DoctorResponse>?> AllDoctors(AllDoctorsRequest request);
        Task<DoctorResponse?> Doctor(Guid guid);
    }
}