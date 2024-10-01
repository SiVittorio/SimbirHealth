using Microsoft.IdentityModel.Tokens;
using SimbirHealth.Data.Models.Account;

namespace SimbirHealth.Account.Services.TokenService
{
    public interface ITokenService
    {
        Task<(string, string)> GenerateTokens(AccountModel account);
        Task<(string, string)?> RefreshToken(string refreshToken);
        Task<TokenValidationResult> ValidateToken(string token);
        Task DeactivateTokens(string accessToken, AccountModel account);
    }
}