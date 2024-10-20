using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        [ProducesResponseType(typeof(List<DoctorResponse>), 200)]
        [ProducesResponseType(404)]
        public async Task<IResult> AllDoctors([FromQuery] AllDoctorsRequest request)
        {
            var doctors = await _doctorService.AllDoctors(request);
            if (!doctors.IsNullOrEmpty())
                return Results.Ok(doctors);
            else
                return Results.NotFound();
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
        [ProducesResponseType(typeof(DoctorResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<IResult> Doctor([FromRoute] Guid id)
        {
            var doctor = await _doctorService.Doctor(id);
            if (doctor != null)
                return Results.Ok(doctor);
            else
                return Results.NotFound();
        }
    }
}
