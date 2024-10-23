using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.IdentityModel.Tokens;
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
            return await _timetableService.PostTimetable(request, GetAccessToken());
        }

        [HttpPut("{id}")]
        [Authorize(Roles = _managerOrAdmin)]
        public async Task<IResult> Timetable([FromRoute]Guid id, [FromBody]AddOrUpdateTimetableRequest request){
            return await _timetableService.PutTimetable(id, request, GetAccessToken());
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = _managerOrAdmin)]
        public async Task<IResult> DeleteTimetable([FromRoute]Guid id){
            return await _timetableService.SoftDeleteTimetable(id);
        }

        [HttpDelete("Doctor/{id}")]
        [Authorize(Roles = _managerOrAdmin)]
        public async Task<IResult> DeleteTimetableByDoctor([FromRoute]Guid id){
            return await _timetableService.SoftDeleteTimetableByDoctor(id);
        }

        [HttpDelete("Hospital/{id}")]
        [Authorize(Roles = _managerOrAdmin)]
        public async Task<IResult> DeleteTimetableByHospital([FromRoute]Guid id){
            return await _timetableService.SoftDeleteTimetableByHospital(id);
        }

        [HttpGet("Hospital/{id}")]
        [Authorize]
        public async Task<IResult> GetTimetableByHospital([FromRoute]Guid id,
            [FromQuery]DateTime from, DateTime to){
            to = DateTime.SpecifyKind(to, DateTimeKind.Utc);
            from = DateTime.SpecifyKind(from, DateTimeKind.Utc);
            
            var timetables = await _timetableService.GetTimetablesByHospital(id, from, to, GetAccessToken());
            if (timetables.IsNullOrEmpty())
                return Results.NotFound();
            return Results.Ok(timetables);
        }


        private string GetAccessToken(){
            return Request.Headers.Authorization.ToString().Replace("Bearer ", "");
        }
    }
}

