using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimbirHealth.Account.Models.Requests.Account;
using SimbirHealth.Account.Models.Responses.Account;
using SimbirHealth.Account.Services.AccountService;
using SimbirHealth.Account.Services.TokenService;
using System.Security.Claims;

namespace SimbirHealth.Account.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ILogger<AccountsController> _logger;
        private readonly IAccountService _accountService;
        public AccountsController(ILogger<AccountsController> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<MeResponse?> Me()
        {
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                IEnumerable<Claim> claims = identity.Claims;
                return await _accountService.Me(ParseGuid(claims));
            }
            else
            {
                return null;
            }
        }

        [HttpPut("[action]")]
        [Authorize]
        public async Task<IResult> Update([FromBody] UpdateRequest request)
        {
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                IEnumerable<Claim> claims = identity.Claims;
                return await _accountService.Update(request, ParseGuid(claims));
            }
            else
            {
                return null;
            }
        }


        private Guid ParseGuid(IEnumerable<Claim> claims)
        {
            return Guid.Parse(claims.FirstOrDefault(c => c.Type == "userGuid")!.Value);
        }
    }
}
