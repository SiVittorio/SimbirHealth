using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SimbirHealth.Account.Models.Requests.Authentication;
using SimbirHealth.Account.Services.TokenService;
using SimbirHealth.Common.Repositories;
using SimbirHealth.Common.Services;
using SimbirHealth.Data.Models;
using SimbirHealth.Data.Models.Account;
using SimbirHealth.Data.Models.Account.NotDbModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SimbirHealth.Account.Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRepositoryBase<AccountModel> _accountRepository;
        private readonly IRepositoryBase<Role> _roleRepository;
        private readonly IRepositoryBase<AccountToRole> _accToRoleRepository;
        private readonly ITokenService _tokenService;

        public AuthenticationService(IRepositoryBase<AccountModel> accountRepository,
            IRepositoryBase<Role> roleRepository,
            IRepositoryBase<AccountToRole> accToRoleRepository,
            ITokenService tokenService)
        {
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
            _accToRoleRepository = accToRoleRepository;
            _tokenService = tokenService;
        }
        /// <summary>
        /// Войти в аккаунт и получить пару JWT (access + refresh)
        /// </summary>
        /// <param name="username">Имя пользователя</param>
        /// <param name="password">Пароль</param>
        /// <returns></returns>
        public async Task<IResult> SignIn(SignInRequest request)
        {
            var account = await _accountRepository.Query()
                .Include(a => a.Roles)
                .Where(a => a.Username == request.Username &&
                a.Password == Hasher.Hash(request.Password)).FirstOrDefaultAsync();
            if (account != null)
            {
                var tokens = await _tokenService.GenerateTokens(account);

                return Results.Ok(new { AccessToken = tokens.Item1, RefreshToken = tokens.Item2 });
            }
            else
                return Results.BadRequest("Неверное имя пользователя или пароль");
        }
        /// <summary>
        /// Зарегистрировать новый аккаунт в системе
        /// </summary>
        /// <param name="request">Запрос на регистрацию, содержащий данные пользователя</param>
        /// <returns></returns>
        public async Task<IResult> SignUp(SignUpRequest request)
        {
            if (await _accountRepository.QueryWithDeleted().AnyAsync(p => p.Username == request.Username))
                return Results.BadRequest("Пользователь с таким Username уже есть");
            else
            {
                var userRole = await _roleRepository.Query().Where(r => r.RoleName == PossibleRoles.User).FirstAsync();
                var user = new AccountModel(request.LastName, request.FirstName, request.Username, Hasher.Hash(request.Password));
                _accToRoleRepository.Add(new() { Account = user, Role = userRole });
                _accountRepository.Add(user);
                await _accountRepository.SaveChangesAsync();

                return Results.Created();
            }
        }
        /// <summary>
        /// Проверка токена на подлинность
        /// </summary>
        /// <param name="token">Access-токен</param>
        /// <returns></returns>
        public async Task<IResult> ValidateToken(string token)
        {
            var validationResult = await _tokenService.ValidateToken(token);
            return validationResult.IsValid ? 
                Results.Ok() : 
                Results.BadRequest(validationResult.Exception.ToString());
        }

        public async Task<IResult> RefreshToken(string refreshToken)
        {
            var tokens = await _tokenService.RefreshToken(refreshToken);
            if (tokens != null)
                return Results.Ok(new { AccessToken = tokens.Value.Item1, RefreshToken = tokens.Value.Item2 });
            else
                return Results.Unauthorized();
        }
        // TODO пока реализован только отзыв refresh-токена, возможно нужно добавить реализацию access-blacklist
        public async Task<IResult> SignOut(string token)
        {
            token = token.Replace("Bearer ", "");
            var validationResult = await _tokenService.ValidateToken(token);
            var claims = validationResult.Claims;
            if (claims.ContainsKey(ClaimTypes.NameIdentifier))
            {
                var accGuid = Guid.Parse((string)claims[ClaimTypes.NameIdentifier]);
                var acc = await _accountRepository
                    .Query()
                    .Where(a => a.Guid == accGuid)
                    .FirstOrDefaultAsync();
                if (acc != null)
                {
                    await _tokenService.DeactivateTokens(token, acc);
                    return Results.Ok();
                }
                else
                {
                    return Results.BadRequest("Не найден владелец токена");
                }

            }
            return Results.BadRequest("Токен не содержит необходимую информацию");
        }
    }
}
