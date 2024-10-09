using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimbirHealth.Common.Services.Account;
using SimbirHealth.Hospital.Models.Requests.Hospital;
using SimbirHealth.Hospital.Services.HospitalService;

namespace SimbirHealth.Hospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalsController : ControllerBase
    {
        private readonly ILogger<HospitalsController> _logger;
        private readonly IHospitalService _hospitalService;

        public HospitalsController(ILogger<HospitalsController> logger,
            IHospitalService hospitalService)
        {
            _logger = logger;
            _hospitalService = hospitalService;
        }
        [HttpGet]
        [Authorize]
        public async Task Hospitals()
        {

        }
        
        [HttpGet("{id}")]
        [Authorize]
        public async Task HospitalById([FromRoute] Guid id)
        {

        }

        [HttpGet("{id}/Rooms")]
        [Authorize]
        public async Task HospitalRooms([FromRoute] Guid id)
        {

        }

        [HttpPost]
        [Authorize(Roles = PossibleRoles.Admin)]
        public async Task<IResult> HospitalsPost([FromBody]AddHospitalRequest request)
        {
            return await _hospitalService.Create(request);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = PossibleRoles.Admin)]
        public async Task HospitalsPut([FromRoute] Guid id)
        {

        }

        [HttpDelete("{id}")]
        [Authorize(Roles = PossibleRoles.Admin)]
        public async Task HospitalsDelete([FromRoute] Guid id)
        {

        }
    }
}
