using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SimbirHealth.Account.Models.Info;
using SimbirHealth.Common.Repositories;
using SimbirHealth.Data.Models.Account;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace SimbirHealth.Account.Services.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly JwtInfo _jwtInfo;
        private readonly IRepositoryBase<RefreshToken> _refreshTokenRepository;

        public TokenService(IOptions<JwtInfo> options,
            IRepositoryBase<RefreshToken> refreshTokenRepository,
            IRepositoryBase<AccountModel> accountsRepository)
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
        /// <summary>
        /// Проверка подписи токена, издателя, времени жизни и алгоритма шифрования
        /// </summary>
        public async Task<IResult> ValidateToken(string token)
        {
            var validationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(new HMACSHA256(Encoding.UTF8.GetBytes(_jwtInfo.SecretKey)).Key),
                ValidIssuer = _jwtInfo.IssuerName,
                ValidateLifetime = true,
                ValidAlgorithms = [SecurityAlgorithms.HmacSha256],
                ValidateAudience = false
            };
            
            try { 
                var claims = new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out var validToken);
                return Results.Ok();
            }
            catch (SecurityTokenValidationException e)
            {
                return Results.BadRequest(e.Message);
            }
        }
        public async Task<(string, string)?> RefreshToken(string refreshToken)
        {
            var tokenModel = await CheckRefreshToken(refreshToken);
            if (tokenModel != null && tokenModel.ExpiredDate > DateTime.UtcNow)
            {
                var acc = tokenModel.Account;
                return await GenerateTokens(acc);
            }
            return null;
        }


        private JwtSecurityToken GenerateAccessToken(AccountModel account)
        {
            Claim[] claims = [new("userGuid", account.Guid.ToString())];
            var sign = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtInfo.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtInfo.IssuerName,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_jwtInfo.AccessLiveHours).AddMinutes(_jwtInfo.AccessLiveMinutes),
                signingCredentials: sign);

            return token;
        }
        private async Task<RefreshToken> CheckRefreshToken(string str)
        {
            var token = await _refreshTokenRepository
                .Query()
                .Include(t => t.Account)
                .FirstOrDefaultAsync(t => t.Token.Contains(str));
            return token;
        }
        private async Task<RefreshToken> GenerateRefreshToken(AccountModel account)
        {
            var oldRefreshToken = await CheckRefreshToken(account.Username);
            if (oldRefreshToken != null)
            {
                _refreshTokenRepository.Delete(oldRefreshToken);
            }
            var token = new RefreshToken(account.Username, DateTime.UtcNow.AddDays(_jwtInfo.RefreshLiveDays), account.Guid);
            _refreshTokenRepository.Add(token);

            await _refreshTokenRepository.SaveChangesAsync();

            return token;
        }
    }
}
