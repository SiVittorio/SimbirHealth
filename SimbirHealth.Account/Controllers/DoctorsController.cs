using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimbirHealth.Account.Models.Requests.Doctor;
using SimbirHealth.Account.Models.Responses.Doctor;
using SimbirHealth.Account.Services.DoctorService;

namespace SimbirHealth.Account.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly ILogger<DoctorsController> _logger;
        private readonly IDoctorService _doctorService;

        public DoctorsController(ILogger<DoctorsController> logger,
            IDoctorService doctorService)
        {
            _logger = logger;
            _doctorService = doctorService;
        }

        /// <summary>
        /// Получение списка докторов
        /// </summary>
        /// <remarks>
        /// Только авторизованные пользователи
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<List<DoctorResponse>?> AllDoctors([FromQuery] AllDoctorsRequest request)
        {
            return await _doctorService.AllDoctors(request);
        }

        /// <summary>
        /// Получение информации о докторе по Id
        /// </summary>
        /// <remarks>
        /// Только авторизованные пользователи
        /// </remarks>
        /// <param name="id">Guid доктора</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<DoctorResponse?> Doctor([FromRoute] Guid id)
        {
            return await _doctorService.Doctor(id);
        }
    }
}
