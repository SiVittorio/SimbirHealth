using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimbirHealth.History.Models.Requests;
using SimbirHealth.History.Models.Responses;
using SimbirHealth.History.Services.HistoryService;

namespace SimbirHealth.History.Controllers
{
    /// <summary>
    /// Контроллер документов.
    /// 
    /// Отвечает за историю посещений пользователей
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryService _historyService;

        public HistoryController(IHistoryService historyService)
        {
            _historyService = historyService;
        }

        [HttpGet("Account/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(List<GetHistoryResponse>), 200)]
        public async Task<IResult> AccountHistories([FromRoute]Guid id){
            return await _historyService.GetAccountHistories(id, GetAccessToken());
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IResult> History(Guid id){
            return await _historyService.GetHistory(id, GetAccessToken());
        }

        [HttpPost]
        [Authorize]
        public async Task<IResult> CreateHistory(AddOrUpdateHistoryRequest request){
            return await _historyService.PostHistory(request, GetAccessToken());
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IResult> UpdateHistory([FromRoute]Guid id, AddOrUpdateHistoryRequest request){
            return await _historyService.PutHistory(id, request, GetAccessToken());
        }

        private string GetAccessToken(){
            return Request.Headers.Authorization.ToString().Replace("Bearer ", "");
        }
    }
}
