using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SimbirHealth.Account.Models.Requests;
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

        public AuthenticationService(IRepositoryBase<AccountModel> accountRepository,
            IRepositoryBase<Role> roleRepository)
        {
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
        }
        public Task<IResult> SignIn(string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult> SignUp(SignUpRequest request)
        {
            var test = await _accountRepository.Query().Include(p => p.Roles).ToListAsync(); 
            if (await _accountRepository.Query().AnyAsync(p => p.Username == request.Username))
                return Results.BadRequest("Пользователь с таким Username уже есть");
            else
            {
                var userRole = await _roleRepository.Query().Where(r => r.RoleName == PossibleRoles.User).FirstAsync();
                _accountRepository.Add(
                    new AccountModel(request.LastName, request.FirstName, request.Username, Hasher.Hash(request.Password), [userRole]));
                await _accountRepository.SaveChangesAsync();

                return Results.Ok();
            }
        }
    }
}
