using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using SimbirHealth.Common.Services.Db.Repositories;
using SimbirHealth.Data.Models.Hospital;
using SimbirHealth.Hospital.Models.Requests.Hospital;
using SimbirHealth.Hospital.Models.Responses.Hospital;

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

        public async Task<List<GetHospitalResponse>> SelectAll(int from, int count)
        {
            return await _hospitalRepository
                .Query()
                .OrderBy(h => h.DateCreate)
                .Skip(from - 1)
                .Take(count)
                .Include(h => h.Rooms)
                .Select(h => 
                    new GetHospitalResponse(
                        h.Name, 
                        h.Address, 
                        h.ContactPhone, 
                        h.Rooms.Select(r => r.Name).ToList())
                        )
                .ToListAsync();
        }


        public async Task<GetHospitalResponse?> SelectById(Guid guid){
            
            var hospitalModel = await HospitalById(guid);

            if (hospitalModel == null){
                return null;   
            }
            else{
                return new GetHospitalResponse(
                    hospitalModel.Name,
                    hospitalModel.Address,
                    hospitalModel.ContactPhone,
                    hospitalModel.Rooms.Select(r => r.Name).ToList());
            }
        }

        public async Task<List<string>> SelectRooms(Guid guid){
            return await _roomRepository
            .Query()
            .Where(r => r.HospitalGuid == guid)
            .Select(r => r.Name)
            .ToListAsync();
        }

        public async Task<IResult> Update(AddHospitalRequest request, Guid guid)
        {
            var hospital = await HospitalById(guid);

            if (hospital == null)
                return Results.BadRequest("Редактируемая больница не найдена");
            
            hospital.Name = request.Name;
            hospital.ContactPhone = request.ContactPhone;
            hospital.Address = request.Address;

            _roomRepository.DeleteRange(hospital.Rooms);

            hospital.Rooms = request.Rooms.Select(r => new Room(r)).ToList();
            
            _roomRepository.AddRange(hospital.Rooms);
            _hospitalRepository.Update(hospital);

            await _hospitalRepository.SaveChangesAsync();
            return Results.Ok();
        }


        public async Task<IResult> SoftDelete(Guid guid){
            var hospital = await HospitalById(guid);

            if (hospital == null)
                return Results.BadRequest("Удаляемая больница не найдена");
            
            hospital.IsDeleted = true;
            hospital.Rooms.ForEach(r => r.IsDeleted = true);
            
            _roomRepository.UpdateRange(hospital.Rooms);
            _hospitalRepository.Update(hospital);

            await _hospitalRepository.SaveChangesAsync();
            return Results.Ok();
        }




        private async Task<HospitalModel?> HospitalById(Guid guid){
            return await _hospitalRepository
            .Query()
            .Include(h => h.Rooms)
            .Where(h => h.Guid == guid)
            .FirstOrDefaultAsync();
        }
    }
}
