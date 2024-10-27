using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        [HttpGet("Account/{id}")]
        [Authorize]
        public async Task<IResult> AccountHistories([FromRoute]Guid id){
            return Results.Ok();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IResult> History(Guid id){
            return Results.Ok();
        }

        [HttpPost]
        [Authorize]
        public async Task<IResult> CreateHistory(){
            return Results.Ok();
        }

        [HttpPut]
        [Authorize]
        public async Task<IResult> UpdateHistory(){
            return Results.Ok();
        }
    }
}
