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
        private const string _managerOrDoctor = PossibleRoles.Manager+","+PossibleRoles.Doctor;

        public TimetableController(ILogger<TimetableController> logger,
        ITimetableService timetableService){
            _logger = logger;
            _timetableService = timetableService;
        }
        
        [HttpPost]
        //FIXME uncomment below
        //[Authorize(Roles = _managerOrDoctor)]
        public async Task<IResult> Timetable(AddOrUpdateTimetableRequest request){
            return await _timetableService.PostTimetable(request, Request.Headers.Authorization.ToString().Replace("Bearer ", ""));
        }
    }
}
