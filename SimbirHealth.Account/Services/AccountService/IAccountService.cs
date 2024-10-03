using SimbirHealth.Account.Models.Requests.Account;
using SimbirHealth.Account.Models.Responses.Account;

namespace SimbirHealth.Account.Services.AccountService
{
    public interface IAccountService
    {
        Task<MeResponse?> Me(Guid guid);
        Task<IResult> Update(UpdateRequest request, Guid guid);
    }
}