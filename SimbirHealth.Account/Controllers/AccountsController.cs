using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimbirHealth.Account.Models.Requests.Account;
using SimbirHealth.Account.Models.Responses.Account;
using SimbirHealth.Account.Services.AccountService;
using SimbirHealth.Account.Services.TokenService;
using SimbirHealth.Data.Models.Account;
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
        /// <summary>
        /// Получение данных о текущем аккаунте
        /// </summary>
        /// <remarks>
        /// Только авторизованные пользователи
        /// </remarks>
        /// <returns></returns>
        [HttpGet("[action]")]
        [Authorize]
        public async Task<MeResponse?> Me()
        {
            _logger.LogWarning(nameof(PossibleRoles.Admin));
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

        /// <summary>
        /// Обновление своего аккаунта
        /// </summary>
        /// <remarks>
        ///  Только авторизованные пользователи
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Получение списка всех аккаунтов
        /// </summary>
        /// <remarks>
        /// Только администраторы
        /// </remarks>
        /// <param name="from">Начало выборки</param>
        /// <param name="count">Конец выборки</param>
        /// <returns>Список аккаунтов</returns>
        [HttpGet]
        [Authorize(Roles = PossibleRoles.Admin)]
        public async Task<List<AccountModel>> Accounts([FromQuery] int from , [FromQuery] int count)
        {
            return await _accountService.SelectAll(from, count);
        }

        /// <summary>
        /// Создание администратором нового аккаунта
        /// </summary>
        /// <remarks>
        /// Только администраторы
        /// </remarks>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = PossibleRoles.Admin)]
        public async Task<IResult> Accounts([FromBody] AdminPostPutAccountRequest request)
        {
            return await _accountService.Create(request);
        }

        /// <summary>
        /// Редактирование администратором аккаунта
        /// </summary>
        /// <remarks>
        /// Только администраторы
        /// </remarks>
        [HttpPut("{id}")]
        [Authorize(Roles = PossibleRoles.Admin)]
        public async Task<IResult> Accounts([FromBody] AdminPostPutAccountRequest request, [FromRoute] Guid id)
        {
            return await _accountService.Update(request, id);
        }

        /// <summary>
        /// Мягкое удаление аккаунта по id
        /// </summary>
        /// <remarks>
        /// Только администраторы
        /// </remarks>
        [HttpDelete("{id}")]
        [Authorize(Roles = PossibleRoles.Admin)]
        public async Task<IResult> Accounts([FromRoute] Guid id)
        {
            return await _accountService.Delete(id);
        }


        private static Guid ParseGuid(IEnumerable<Claim> claims)
        {
            return Guid.Parse(claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);
        }
    }
}
