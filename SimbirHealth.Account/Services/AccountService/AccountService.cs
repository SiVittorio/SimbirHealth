using Microsoft.EntityFrameworkCore;
using SimbirHealth.Account.Models.Requests.Account;
using SimbirHealth.Account.Models.Responses.Account;
using SimbirHealth.Common.Repositories;
using SimbirHealth.Common.Services;
using SimbirHealth.Data.Models.Account;

namespace SimbirHealth.Account.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly IRepositoryBase<AccountModel> _accountRepository;

        public AccountService(IRepositoryBase<AccountModel> accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<MeResponse?> Me(Guid guid)
        {
            AccountModel? account = await TakeAccount(guid);
            return account != null ? new(
                account.FirstName,
                account.LastName,
                account.Username) : null;
        }

        public async Task<IResult> Update(UpdateRequest request, Guid guid)
        {
            AccountModel? account = await TakeAccount(guid);

            if (account != null) {
                account.FirstName = request.FirstName;
                account.LastName = request.LastName;
                account.Password = Hasher.Hash(request.Password); 

                _accountRepository.Update(account);
                await _accountRepository.SaveChangesAsync();

                return Results.Ok();
            }
            else
            {
                return Results.BadRequest();
            }
        }

        private async Task<AccountModel?> TakeAccount(Guid guid)
        {
            return await _accountRepository.Query().FirstOrDefaultAsync(a => a.Guid == guid);
        }
    }
}
