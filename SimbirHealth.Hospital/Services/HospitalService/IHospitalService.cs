using SimbirHealth.Hospital.Models.Requests.Hospital;
using SimbirHealth.Hospital.Models.Responses.Hospital;

namespace SimbirHealth.Hospital.Services.HospitalService
{
    public interface IHospitalService
    {
        Task<IResult> Create(AddHospitalRequest request);
        Task<List<GetHospitalResponse>> SelectAll(int from, int count);
        Task<GetHospitalResponse?> SelectById(Guid guid);
        Task<List<string>> SelectRooms(Guid guid);
        Task<IResult> Update(AddHospitalRequest request, Guid guid);
        Task<IResult> SoftDelete(Guid guid);
    }
}