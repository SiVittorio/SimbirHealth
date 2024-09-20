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

        [HttpPost("[action]")]
        public async Task SignIn()
        {
            throw new NotImplementedException();
        }

        [HttpPut("[action]")]
        public async Task SignOut()
        {
            throw new NotImplementedException();
        }

        [HttpGet("[action]")]
        public async Task Validate()
        {
            throw new NotImplementedException();
        }

        [HttpPost("[action]")]
        public async Task Refresh()
        {
            throw new NotImplementedException();
        }
    }
}
