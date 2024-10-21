using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimbirHealth.Common.Services.Account;
using SimbirHealth.Data.SharedResponses.Hospital;
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
        /// <summary>
        /// Получение списка больниц
        /// </summary>
        /// <remarks>
        /// Только авторизованные пользователи
        /// </remarks>
        [HttpGet]
        [Authorize]
        public async Task<List<HospitalResponse>> Hospitals([FromQuery] int from , [FromQuery] int count)
        {
            return await _hospitalService.SelectAll(from, count);
        }
        /// <summary>
        /// Получение информации о больнице по Id
        /// </summary>
        /// <remarks>
        /// Только авторизованные пользователи
        /// </remarks>
        /// <param name="id">Id больницы</param>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(HospitalResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<IResult> HospitalById([FromRoute] Guid id)
        {
            var hospital = await _hospitalService.SelectById(id);
            if (hospital != null)
                return Results.Ok(hospital);
            else
                return Results.NotFound();
        }
        /// <summary>
        /// Получение списка кабинетов больницы по Id
        /// </summary>
        /// <remarks>
        /// Только авторизованные пользователи
        /// </remarks>
        /// <param name="id">Id больницы</param>
        [HttpGet("{id}/Rooms")]
        [Authorize]
        public async Task<List<string>> HospitalRooms([FromRoute] Guid id)
        {
            return await _hospitalService.SelectRooms(id);
        }
        /// <summary>
        /// Создание записи о новой больнице
        /// </summary>
        /// <remarks>
        /// Только администраторы
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = PossibleRoles.Admin)]
        public async Task<IResult> HospitalsPost([FromBody]AddHospitalRequest request)
        {
            return await _hospitalService.Create(request);
        }
        /// <summary>
        /// Изменение информации о больнице по Id
        /// </summary>
        /// <remarks>
        /// Только администраторы
        /// </remarks>
        [HttpPut("{id}")]
        [Authorize(Roles = PossibleRoles.Admin)]
        public async Task<IResult> HospitalsPut([FromBody] AddHospitalRequest request, [FromRoute] Guid id)
        {
            return await _hospitalService.Update(request, id);   
        }
        /// <summary>
        /// Мягкое удаление записи о больнице
        /// </summary>
        /// <remarks>
        /// Только администраторы
        /// </remarks>
        [HttpDelete("{id}")]
        [Authorize(Roles = PossibleRoles.Admin)]
        public async Task<IResult> HospitalsDelete([FromRoute] Guid id)
        {
            return await _hospitalService.SoftDelete(id);
        }
    }
}
