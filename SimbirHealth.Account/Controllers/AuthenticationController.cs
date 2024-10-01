using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimbirHealth.Account.Models.Requests;
using SimbirHealth.Account.Services.AuthenticationService;

namespace SimbirHealth.Account.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(ILogger<AuthenticationController> logger,
            IAuthenticationService authenticationService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Регистрация нового аккаунта
        /// </summary>
        [HttpPost("[action]")]
        public async Task<IResult> SignUp(SignUpRequest request)
        {
            return await _authenticationService.SignUp(request);
        }
        /// <summary>
        /// Получение новой пары jwt пользователя
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IResult> SignIn(SignInRequest request)
        {
            return await _authenticationService.SignIn(request);
        }
        /// <summary>
        /// Выход из аккаунта
        /// </summary>
        /// <remarks>
        /// Только авторизованные пользователи
        /// </remarks>
        /// <returns></returns>
        [HttpPut("[action]")]
        [Authorize]
        public async Task<IResult> SignOut()
        {
            return Results.Ok();
        }
        /// <summary>
        /// Интроспекция токена
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IResult> Validate([FromQuery] string accessToken)
        {
            return await _authenticationService.ValidateToken(accessToken);
        }
        /// <summary>
        ///  Обновление пары токенов
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IResult> Refresh([FromBody] string refreshToken)
        {
            return await _authenticationService.RefreshToken(refreshToken);
        }
    }
}
