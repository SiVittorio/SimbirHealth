using Microsoft.AspNetCore.Mvc;

namespace SimbirHealth.Accounts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(ILogger<AuthenticationController> logger)
        {
            _logger = logger;
        }

        [HttpPost("[action]")]
        public async Task SignUp()
        {
            throw new NotImplementedException();
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
