using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimbirHealth.Common.Services.Account;

namespace SimbirHealth.Hospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalsController : ControllerBase
    {
        private readonly ILogger<HospitalsController> _logger;

        public HospitalsController(ILogger<HospitalsController> logger)
        {
            _logger = logger;
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
        public async Task HospitalsPost()
        {

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
