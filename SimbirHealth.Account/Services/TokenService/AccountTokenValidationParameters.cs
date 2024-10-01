using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SimbirHealth.Account.Models.Info;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime;
using System.Security.Cryptography;
using System.Text;

namespace SimbirHealth.Account.Services.TokenService
{
    public static class AccountTokenValidationParameters
    {
        public static TokenValidationParameters DefaultParameters(JwtInfo jwtInfo) => new()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(new HMACSHA256(Encoding.UTF8.GetBytes(jwtInfo.SecretKey)).Key),
            ValidIssuer = jwtInfo.IssuerName,
            ValidateLifetime = true,
            ValidAlgorithms = [SecurityAlgorithms.HmacSha256],
            ValidateAudience = false
        };
    }
}
