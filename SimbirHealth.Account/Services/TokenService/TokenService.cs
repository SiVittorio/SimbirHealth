﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SimbirHealth.Common.Services.Account;
using SimbirHealth.Common.Services.Db.Repositories;
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
            IRepositoryBase<RefreshToken> refreshTokenRepository)
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
        public async Task<TokenValidationResult> ValidateToken(string token)
        {
            var result = await new JwtSecurityTokenHandler().ValidateTokenAsync(token,
                    AccountTokenValidationParameters.DefaultParameters(_jwtInfo));
            return result;
        }
        /// <summary>
        /// Обновить access-токен пользователя, также обновить его refresh-токен в БД
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
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
        public async Task DeactivateTokens(string accessToken, AccountModel account)
        {
            await RemoveRefreshToken(account);
        }



        private JwtSecurityToken GenerateAccessToken(AccountModel account)
        {
            IEnumerable<Claim> claims = [
                new(ClaimTypes.NameIdentifier, account.Guid.ToString())//,
                //new(ClaimTypes.Role, account.RolesToString())
            ];

            foreach (var role in account.Roles)
            {
                claims = claims.Append(new(ClaimTypes.Role, role.RoleName));
            }

            var sign = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtInfo.SecretKey)),
                SecurityAlgorithms.HmacSha256);
            #if DEBUG
            var token = new JwtSecurityToken(
                issuer: _jwtInfo.IssuerName,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: sign);
            #else
            var token = new JwtSecurityToken(
                issuer: _jwtInfo.IssuerName,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_jwtInfo.AccessLiveHours).AddMinutes(_jwtInfo.AccessLiveMinutes),
                signingCredentials: sign);
            #endif

            return token;
        }
        private async Task<RefreshToken> CheckRefreshToken(string str)
        {
            var token = await _refreshTokenRepository
                .Query()
                .Include(t => t.Account)
                .ThenInclude(a => a.Roles)
                .FirstOrDefaultAsync(t => t.Token.Contains(str));
            return token;
        }
        private async Task<RefreshToken> GenerateRefreshToken(AccountModel account)
        {
            RefreshToken? oldRefreshToken = await RemoveRefreshToken(account);
            var token = new RefreshToken(account.Username,
                DateTime.UtcNow.AddDays(_jwtInfo.RefreshLiveDays),
                account.Guid);
            _refreshTokenRepository.Add(token);

            await _refreshTokenRepository.SaveChangesAsync();

            return token;
        }

        private async Task<RefreshToken?> RemoveRefreshToken(AccountModel account)
        {
            var oldRefreshToken = await CheckRefreshToken(account.Username);
            if (oldRefreshToken != null)
            {
                _refreshTokenRepository.Delete(oldRefreshToken);
                await _refreshTokenRepository.SaveChangesAsync();
            }
            return oldRefreshToken;
        }
    }
}
