using SimbirHealth.Data.Models.Account;

namespace SimbirHealth.Account.Services.TokenService
{
    public interface ITokenService
    {
        Task<(string, string)> GenerateTokens(AccountModel account);
        Task<(string, string)?> RefreshToken(string refreshToken);
        Task<IResult> ValidateToken(string token);
    }
}