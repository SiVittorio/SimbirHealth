using Microsoft.EntityFrameworkCore;
using SimbirHealth.Account.Models.Requests.Doctor;
using SimbirHealth.Common.Services.Account;
using SimbirHealth.Common.Services.Db.Repositories;
using SimbirHealth.Data.Models.Account;
using SimbirHealth.Data.SharedResponses.Account;

namespace SimbirHealth.Account.Services.DoctorService
{
    public class DoctorService : IDoctorService
    {
        private readonly IRepositoryBase<AccountModel> _accountRepository;
        private readonly Role _doctorRole;

        public DoctorService(IRepositoryBase<AccountModel> accountRepository,
            IRepositoryBase<Role> roleRepository)
        {
            _accountRepository = accountRepository;
            _doctorRole = roleRepository.Query().Where(r => r.RoleName == PossibleRoles.Doctor).First();
        }

        /// <summary>
        /// Получить всех врачей по фильтру
        /// </summary>
        public async Task<List<DoctorResponse>?> AllDoctors(AllDoctorsRequest request)
        {
            return await _accountRepository
                .Query()
                .Where(a => a.Roles.Contains(_doctorRole)
                    && a.FirstName.Contains(request.NameFilter))
                .OrderBy(a => a.DateCreate)
                .Skip(request.From - 1)
                .Take(request.Count)
                .Select(a => new DoctorResponse(a.Guid, a.FirstName, a.LastName))
                .ToListAsync();
        }

        /// <summary>
        /// Получить информацию о враче по Guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public async Task<DoctorResponse?> Doctor(Guid guid)
        {
            return await _accountRepository
                .Query()
                .Where(a => a.Roles.Contains(_doctorRole) && a.Guid == guid)
                .Select(a => new DoctorResponse(a.Guid, a.FirstName, a.LastName))
                .FirstOrDefaultAsync();
        }
    }
}
