using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SimbirHealth.Account.Models.Info;
using SimbirHealth.Data.Models.Account;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SimbirHealth.Account.Services.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly JwtInfo _jwtInfo;

        public TokenService(IOptions<JwtInfo> options)
        {
            _jwtInfo = options.Value;
        }
        public string GenerateToken(AccountModel account)
        {
            var sign = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtInfo.SecretKey)),
                SecurityAlgorithms.Aes128CbcHmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtInfo.IssuerName,
                claims: [new("userGuid", account.Guid.ToString())],
                expires: DateTime.UtcNow.AddHours(_jwtInfo.LiveHours).AddMinutes(_jwtInfo.LiveMinutes),
                signingCredentials: sign);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
