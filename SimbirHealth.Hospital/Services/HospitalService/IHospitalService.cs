using SimbirHealth.Hospital.Models.Requests.Hospital;

namespace SimbirHealth.Hospital.Services.HospitalService
{
    public interface IHospitalService
    {
        Task<IResult> Create(AddHospitalRequest request);
    }
}