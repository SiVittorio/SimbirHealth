using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimbirHealth.Common.Services.Account;
using SimbirHealth.Hospital.Models.Requests.Hospital;
using SimbirHealth.Hospital.Models.Responses.Hospital;
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
        public async Task<List<GetHospitalResponse>> Hospitals([FromQuery] int from , [FromQuery] int count)
        {
            return await _hospitalService.SelectAll(from, count);
        }
        
        [HttpGet("{id}")]
        [Authorize]
        public async Task<GetHospitalResponse?> HospitalById([FromRoute] Guid id)
        {
            return await _hospitalService.SelectById(id);
        }

        [HttpGet("{id}/Rooms")]
        [Authorize]
        public async Task<List<string>> HospitalRooms([FromRoute] Guid id)
        {
            return await _hospitalService.SelectRooms(id);
        }

        [HttpPost]
        [Authorize(Roles = PossibleRoles.Admin)]
        public async Task<IResult> HospitalsPost([FromBody]AddHospitalRequest request)
        {
            return await _hospitalService.Create(request);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = PossibleRoles.Admin)]
        public async Task<IResult> HospitalsPut([FromBody] AddHospitalRequest request, [FromRoute] Guid id)
        {
            return await _hospitalService.Update(request, id);   
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = PossibleRoles.Admin)]
        public async Task<IResult> HospitalsDelete([FromRoute] Guid id)
        {
            return await _hospitalService.SoftDelete(id);
        }
    }
}
