using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SimbirHealth.Account.Models.Requests;
using SimbirHealth.Account.Services.TokenService;
using SimbirHealth.Common.Repositories;
using SimbirHealth.Common.Services;
using SimbirHealth.Data.Models;
using SimbirHealth.Data.Models.Account;

namespace SimbirHealth.Account.Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRepositoryBase<AccountModel> _accountRepository;
        private readonly IRepositoryBase<Role> _roleRepository;
        private readonly IRepositoryBase<AccountToRole> _accToRoleRepository;
        private readonly ITokenService _tokenService;

        public AuthenticationService(IRepositoryBase<AccountModel> accountRepository,
            IRepositoryBase<Role> roleRepository,
            IRepositoryBase<AccountToRole> accToRoleRepository,
            ITokenService tokenService)
        {
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
            _accToRoleRepository = accToRoleRepository;
            _tokenService = tokenService;
        }
        /// <summary>
        /// Войти в аккаунт и получить пару JWT (access + refresh)
        /// </summary>
        /// <param name="username">Имя пользователя</param>
        /// <param name="password">Пароль</param>
        /// <returns></returns>
        public async Task<IResult> SignIn(string username, string password)
        {
            var account = await _accountRepository.Query()
                .Include(a => a.Roles)
                .Where(a => a.Username == username &&
                a.Password == Hasher.Hash(password)).FirstOrDefaultAsync();
            if (account != null)
            {
                var tokens = await _tokenService.GenerateTokens(account);

                return Results.Ok(new {tokens.Item1, tokens.Item2});
            }
            else
                return Results.BadRequest("Неверное имя пользователя или пароль");
        }
        /// <summary>
        /// Зарегистрировать новый аккаунт в системе
        /// </summary>
        /// <param name="request">Запрос на регистрацию, содержащий данные пользователя</param>
        /// <returns></returns>
        public async Task<IResult> SignUp(SignUpRequest request)
        {
            var test = await _accountRepository.Query().Include(p => p.Roles).ToListAsync(); 
            if (await _accountRepository.Query().AnyAsync(p => p.Username == request.Username))
                return Results.BadRequest("Пользователь с таким Username уже есть");
            else
            {
                var userRole = await _roleRepository.Query().Where(r => r.RoleName == PossibleRoles.User).FirstAsync();
                var user = new AccountModel(request.LastName, request.FirstName, request.Username, Hasher.Hash(request.Password));
                _accToRoleRepository.Add( new() { Account = user, Role = userRole } );
                _accountRepository.Add(user);
                await _accountRepository.SaveChangesAsync();

                return Results.Ok();
            }
        }
    }
}
