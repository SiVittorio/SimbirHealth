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
        public async Task<IResult> SignIn(string username, string password)
        {
            var account = await _accountRepository.Query().Where(a => a.Username == username &&
                a.Password == Hasher.Hash(password)).FirstOrDefaultAsync();
            if (account != null)
            {
                return Results.Ok(_tokenService.GenerateToken(account));
            }
            else
                return Results.BadRequest("Неверное имя пользователя или пароль");
        }

        public async Task<IResult> SignUp(SignUpRequest request)
        {
            var test = await _accountRepository.Query().Include(p => p.Roles).ToListAsync(); 
            if (await _accountRepository.Query().AnyAsync(p => p.Username == request.Username))
                return Results.BadRequest("Пользователь с таким Username уже есть");
            else
            {
                var userRole = await _roleRepository.Query().Where(r => r.RoleName == PossibleRoles.User).FirstAsync();
                var user = new AccountModel(request.LastName, request.FirstName, request.Username, Hasher.Hash(request.Password), [userRole]);
                _accToRoleRepository.Add( new() { Account = user, Role = userRole } );
                _accountRepository.Add(user);
                await _accountRepository.SaveChangesAsync();

                return Results.Ok();
            }
        }
    }
}
