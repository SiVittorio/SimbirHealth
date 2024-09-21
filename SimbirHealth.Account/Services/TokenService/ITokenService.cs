using SimbirHealth.Data.Models.Account;

namespace SimbirHealth.Account.Services.TokenService
{
    public interface ITokenService
    {
        string GenerateToken(AccountModel account);
    }
}