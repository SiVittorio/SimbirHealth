using SimbirHealth.Common.Services.Db.Repositories;
using SimbirHealth.Data.Models.Hospital;
using SimbirHealth.Hospital.Models.Requests.Hospital;

namespace SimbirHealth.Hospital.Services.HospitalService
{
    public class HospitalService : IHospitalService
    {
        private readonly IRepositoryBase<HospitalModel> _hospitalRepository;
        private readonly IRepositoryBase<Room> _roomRepository;

        public HospitalService(IRepositoryBase<HospitalModel> hospitalRepository, IRepositoryBase<Room> roomRepository)
        {
            _hospitalRepository = hospitalRepository;
            _roomRepository = roomRepository;
        }

        /// <summary>
        /// Создание новой больницы в БД
        /// </summary>
        public async Task<IResult> Create(AddHospitalRequest request)
        {
            _hospitalRepository.Add(new
                (
                request.Name,
                request.Address,
                request.ContactPhone,
                request.Rooms.Select(r => new Room(r)).ToList()
                )
            );

            await _hospitalRepository.SaveChangesAsync();
            return Results.Created();
        }
    }
}
