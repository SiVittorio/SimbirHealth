using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace SimbirHealth.Data.Models.Account.NotDbModel
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
