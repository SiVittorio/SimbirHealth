using SimbirHealth.Data.SharedResponses.Hospital;
using SimbirHealth.Hospital.Models.Requests.Hospital;

namespace SimbirHealth.Hospital.Services.HospitalService
{
    public interface IHospitalService
    {
        Task<IResult> Create(AddHospitalRequest request);
        Task<List<HospitalResponse>> SelectAll(int from, int count);
        Task<HospitalResponse?> SelectById(Guid guid);
        Task<List<RoomResponse>> SelectRooms(Guid guid);
        Task<IResult> Update(AddHospitalRequest request, Guid guid);
        Task<IResult> SoftDelete(Guid guid);
    }
}