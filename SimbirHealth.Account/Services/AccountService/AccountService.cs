using Microsoft.EntityFrameworkCore;
using SimbirHealth.Account.Models.Requests.Account;
using SimbirHealth.Account.Models.Responses.Account;
using SimbirHealth.Common.Services.Db;
using SimbirHealth.Common.Services.Db.Repositories;
using SimbirHealth.Data.Models.Account;
using System.Data;
using System.Security.Principal;

namespace SimbirHealth.Account.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly IRepositoryBase<AccountModel> _accountRepository;
        private readonly IRepositoryBase<Role> _roleRepository;
        private readonly IRepositoryBase<AccountToRole> _accountToRoleRepository;

        public AccountService(IRepositoryBase<AccountModel> accountRepository,
            IRepositoryBase<Role> roleRepository, 
            IRepositoryBase<AccountToRole> accountToRoleRepository)
        {
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
            _accountToRoleRepository = accountToRoleRepository;
        }
        /// <summary>
        /// Получить информацию об аккаунте по Guid
        /// </summary>
        /// <param name="guid">Guid аккаунта</param>
        public async Task<MeResponse?> Me(Guid guid)
        {
            AccountModel? account = await TakeAccount(guid);
            return account != null ? new(
                account.FirstName,
                account.LastName,
                account.Username) : null;
        }

        /// <summary>
        /// Обновить информацию об аккаунте.
        /// Простой вариант (для обычных пользователей)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public async Task<IResult> Update(UpdateRequest request, Guid guid)
        {
            AccountModel? account = await TakeAccount(guid);

            if (account == null)
                return Results.BadRequest("Пользователь не найден");

            account.FirstName = request.FirstName;
            account.LastName = request.LastName;
            account.Password = Hasher.Hash(request.Password);

            _accountRepository.Update(account);
            await _accountRepository.SaveChangesAsync();

            return Results.Ok();
        }

        /// <summary>
        /// Получить список существующих аккаунтов,
        /// выборка начинается с конкретной сущности и берется определенное их число
        /// </summary>
        /// <param name="from">Начиная с какой сущности брать</param>
        /// <param name="count">Максимальное число сущностей</param>
        public async Task<List<AccountModel>> SelectAll(int from, int count)
        {
            return await _accountRepository
                .QueryWithDeleted()
                .OrderBy(a => a.DateCreate)
                .Skip(from - 1)
                .Take(count)
                .Include(a => a.Roles)
                .ToListAsync();
        }

        /// <summary>
        /// Добавление нового пользователя администратором 
        /// </summary>
        /// <param name="request">Запрос с данными нового аккаунта</param>
        public async Task<IResult> Create(AdminPostPutAccountRequest request)
        {
            if (await _accountRepository.QueryWithDeleted().AnyAsync(p => p.Username == request.Username))
                return Results.BadRequest("Пользователь с таким Username уже есть");

            AccountModel account = new AccountModel(
                    request.LastName,
                    request.FirstName,
                    request.Username,
                    Hasher.Hash(request.Password));

            var roles = new List<Role>();
            var links = new List<AccountToRole>();
            try { (roles, links) = await ValidateRoles(request.Roles, account); }
            catch (Exception ex) { return Results.BadRequest(ex.Message); }

            account.Roles = roles;
            _accountToRoleRepository.AddRange(links);

            await _accountRepository.SaveChangesAsync();
            return Results.Created();
        }

        /// <summary>
        /// Обновление пользователя администратором 
        /// </summary>
        /// <param name="request">Запрос с данными</param>
        /// <param name="guid">Guid обновляемого аккаунта</param>
        /// <returns></returns>
        public async Task<IResult> Update(AdminPostPutAccountRequest request, Guid guid)
        {
            var account = _accountRepository.QueryWithDeleted()
                .Include(a => a.AccountToRoles)
                .FirstOrDefault(a => a.Guid == guid);
            
            if (account == null) 
                return Results.BadRequest("Редактируемый пользователь не найден");

            account.LastName = request.LastName;
            account.FirstName = request.FirstName;
            account.Password = Hasher.Hash(request.Password);
            account.Username = request.Username;

            var roles = new List<Role>();
            var links = new List<AccountToRole>();
            try { (roles, links) = await ValidateRoles(request.Roles, account); }
            catch (Exception ex) { return Results.BadRequest(ex.Message); }

            _accountToRoleRepository.DeleteRange(account.AccountToRoles!);

            account.Roles = roles;
            _accountToRoleRepository.AddRange(links);
            _accountRepository.Update(account);

            await _accountRepository.SaveChangesAsync();
            return Results.Ok();
        }

        /// <summary>
        /// Мягкое удаление пользователя
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public async Task<IResult> SoftDelete(Guid guid)
        {
            var acc = await TakeAccount(guid);
            if (acc == null)
                return Results.BadRequest("Удаляемый пользователь не найден");

            acc.IsDeleted = true;
            _accountRepository.Update(acc);
            await _accountRepository.SaveChangesAsync();

            return Results.Ok();

        }








        private async Task<AccountModel?> TakeAccount(Guid guid)
        {
            return await _accountRepository.Query().FirstOrDefaultAsync(a => a.Guid == guid);
        }

        private async Task<(List<Role> roles, List<AccountToRole> links)> ValidateRoles(List<string> strRoles, AccountModel owner)
        {
            var roles = new List<Role>();
            var links = new List<AccountToRole>();
            foreach (var role in strRoles)   
            {
                var roleDb = await _roleRepository.Query().FirstOrDefaultAsync(r => r.RoleName == role);
                if (roleDb != null)
                {
                    roles.Add(roleDb);
                    links.Add(new() { Account = owner, Role = roleDb });
                }
                else
                {
                    throw new Exception("Выбрана несуществующая роль");
                }
            }
            return (roles, links);
        }
    }
}
