using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SimbirHealth.Accounts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly ILogger<DoctorsController> _logger;

        public DoctorsController(ILogger<DoctorsController> logger)
        {
            _logger = logger;
        }
    }
}
