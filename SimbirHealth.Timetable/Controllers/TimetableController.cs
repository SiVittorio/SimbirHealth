using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimbirHealth.Common.Services.Account;
using SimbirHealth.Timetable.Models.Requests;
using SimbirHealth.Timetable.Services.ExternalApiService;
using SimbirHealth.Timetable.Services.TimeValidatorService;

namespace SimbirHealth.Timetable.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimetableController : ControllerBase
    {
        //private readonly TimetableService _timetableService;
        private readonly ILogger<TimetableController> _logger;
        private readonly IExternalApiService _externalApiService;

        private const string _managerOrDoctor = PossibleRoles.Manager+","+PossibleRoles.Doctor;

        public TimetableController(//TimeTableService timetableService,
        ILogger<TimetableController> logger,
        IExternalApiService externalApiService){
            //_timetableService = timetableService;
            _logger = logger;
            _externalApiService = externalApiService;
        }
        //eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImRmYTNlYTk1LTIxZTEtNDRjNi05MzkzLTVhYjUzMWQzOWFjZCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6Ik1hbmFnZXIiLCJleHAiOjE3MzE5NDAzOTAsImlzcyI6IlNpbWJpckhlYWx0aEJhY2tlbmQifQ.XVFphKr0fasn09GtV0_I02pD5pjgsK1lHAm-wIE3640
        [HttpPost]
        [Authorize(Roles = _managerOrDoctor)]
        public async Task<IResult> Timetable(AddOrUpdateTimetableRequest request){
            var resp = _externalApiService.GetDoctorByGuid
            (request.DoctorId, 
            Request
                .Headers
                .Authorization
                .ToString()
                .Replace("Bearer ", ""));
            return Results.Ok(resp);
        }
    }
}
