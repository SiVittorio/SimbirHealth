using SimbirHealth.Account.Models.Requests.Account;
using SimbirHealth.Account.Models.Responses.Account;
using SimbirHealth.Data.Models.Account;

namespace SimbirHealth.Account.Services.AccountService
{
    public interface IAccountService
    {
        Task<MeResponse?> Me(Guid guid);
        Task<IResult> Update(UpdateRequest request, Guid guid);
        Task<List<AccountModel>> SelectAll(int from, int count);
        Task<IResult> Create(AdminPostPutAccountRequest request);
        Task<IResult> Update(AdminPostPutAccountRequest request, Guid id);
        Task<IResult> SoftDelete(Guid guid);
    }
}