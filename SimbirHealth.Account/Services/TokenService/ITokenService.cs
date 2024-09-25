using SimbirHealth.Data.Models.Account;

namespace SimbirHealth.Account.Services.TokenService
{
    public interface ITokenService
    {
        Task<(string, string)> GenerateTokens(AccountModel account);
    }
}