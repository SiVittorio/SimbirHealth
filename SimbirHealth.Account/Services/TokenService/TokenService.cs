using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SimbirHealth.Account.Models.Info;
using SimbirHealth.Common.Repositories;
using SimbirHealth.Data.Models.Account;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SimbirHealth.Account.Services.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly JwtInfo _jwtInfo;
        private readonly IRepositoryBase<RefreshToken> _refreshTokenRepository;

        public TokenService(IOptions<JwtInfo> options, IRepositoryBase<RefreshToken> refreshTokenRepository)
        {
            _jwtInfo = options.Value;
            _refreshTokenRepository = refreshTokenRepository;
        }
        /// <summary>
        /// Сгенерировать пару JWT (access + refresh)
        /// </summary>
        /// <param name="account">Данные аккаунта</param>
        /// <returns></returns>
        public async Task<(string, string)> GenerateTokens(AccountModel account)
        {
            var accessToken = GenerateAccessToken(account);
            var refreshToken = await GenerateRefreshToken(account);
            
            return (new JwtSecurityTokenHandler().WriteToken(accessToken), refreshToken.Token);
        }

        private JwtSecurityToken GenerateAccessToken(AccountModel account)
        {
            Claim[] claims = [new("userGuid", account.Guid.ToString())];
            var sign = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtInfo.SecretKey)),
                SecurityAlgorithms.Aes128CbcHmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtInfo.IssuerName,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_jwtInfo.LiveHours).AddMinutes(_jwtInfo.LiveMinutes),
                signingCredentials: sign);

            return token;
        }
        private async Task<RefreshToken> GenerateRefreshToken(AccountModel account)
        {
            var oldRefreshToken = await _refreshTokenRepository
                .Query()
                .FirstOrDefaultAsync(t => t.Token.Contains(account.Username));
            if (oldRefreshToken != null)
            {
                _refreshTokenRepository.Delete(oldRefreshToken);
            }
            var token = new RefreshToken(account.Username);
            _refreshTokenRepository.Add(token);

            await _refreshTokenRepository.SaveChangesAsync();

            return token;
        }
    }
}
