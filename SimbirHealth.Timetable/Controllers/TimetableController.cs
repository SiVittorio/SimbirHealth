using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimbirHealth.Common.Services.Account;
using SimbirHealth.Timetable.Models.Requests;
using SimbirHealth.Timetable.Services.ExternalApiService;
using SimbirHealth.Timetable.Services.TimetableService;
using SimbirHealth.Timetable.Services.TimeValidator;

namespace SimbirHealth.Timetable.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimetableController : ControllerBase
    {
        private readonly ILogger<TimetableController> _logger;
        private readonly ITimetableService _timetableService;
        private const string _managerOrAdmin = PossibleRoles.Manager+","+PossibleRoles.Admin;

        public TimetableController(ILogger<TimetableController> logger,
        ITimetableService timetableService){
            _logger = logger;
            _timetableService = timetableService;
        }
        
        [HttpPost]
        [Authorize(Roles = _managerOrAdmin)]
        public async Task<IResult> Timetable([FromBody]AddOrUpdateTimetableRequest request){
            return await _timetableService.PostTimetable(request, Request.Headers.Authorization.ToString().Replace("Bearer ", ""));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = _managerOrAdmin)]
        public async Task<IResult> Timetable([FromRoute]Guid id, [FromBody]AddOrUpdateTimetableRequest request){
            return Results.Ok();
        }
    }
}

