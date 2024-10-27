using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SimbirHealth.Common.Services.Account;
using SimbirHealth.Data.Models.Timetable;
using SimbirHealth.Timetable.Models.Requests;
using SimbirHealth.Timetable.Models.Responses;
using SimbirHealth.Timetable.Services.TimetableService;

namespace SimbirHealth.Timetable.Controllers
{
    /// <summary>
    /// Микросервис расписаний
    /// 
    /// Отвечает за расписание врачей и
    /// больниц, а также за запись на приём пользователем. 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TimetableController : ControllerBase
    {
        private readonly ILogger<TimetableController> _logger;
        private readonly ITimetableService _timetableService;
        private const string _managerOrAdmin = PossibleRoles.Manager+","+PossibleRoles.Admin;
        private const string _managerOrAdminOrDoctor = _managerOrAdmin+","+PossibleRoles.Doctor;

        public TimetableController(ILogger<TimetableController> logger,
        ITimetableService timetableService){
            _logger = logger;
            _timetableService = timetableService;
        }
        /// <summary>
        /// Создание новой записи в расписании
        /// </summary>
        /// <remarks>
        /// Только администраторы и менеджеры. {from} и {to} - количество
        /// минут всегда кратно 30, секунды всегда 0 (пример: “2024-04-25T11:30:00Z”, “2024-
        /// 04-25T12:00:00Z”). {to} > {from}. Разница между {to} и {from} не должна превышать
        /// 12 часов.
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = _managerOrAdmin)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IResult> Timetable([FromBody]AddOrUpdateTimetableRequest request){
            return await _timetableService.PostTimetable(request, GetAccessToken());
        }
        /// <summary>
        /// Обновление записи расписания
        /// </summary>
        /// <remarks>
        /// Только администраторы и менеджеры. 
        /// Нельзя изменить если есть записавшиеся на прием.
        /// {from} и {to} - количество
        /// минут всегда кратно 30, секунды всегда 0 (пример: “2024-04-25T11:30:00Z”, “2024-
        /// 04-25T12:00:00Z”). {to} > {from}. Разница между {to} и {from} не должна превышать
        /// 12 часов.
        /// </remarks>
        [HttpPut("{id}")]
        [Authorize(Roles = _managerOrAdmin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IResult> Timetable([FromRoute]Guid id, [FromBody]AddOrUpdateTimetableRequest request){
            return await _timetableService.PutTimetable(id, request, GetAccessToken());
        }
        /// <summary>
        /// Удаление записи расписания
        /// </summary>
        /// <remarks>Только администраторы и менеджеры</remarks>
        [HttpDelete("{id}")]
        [Authorize(Roles = _managerOrAdmin)]
        public async Task<IResult> DeleteTimetable([FromRoute]Guid id){
            return await _timetableService.SoftDeleteTimetable(id);
        }
        /// <summary>
        /// Удаление записей расписания доктора
        /// </summary>
        /// <remarks>Только администраторы и менеджеры</remarks>
        [HttpDelete("Doctor/{id}")]
        [Authorize(Roles = _managerOrAdmin)]
        public async Task<IResult> DeleteTimetableByDoctor([FromRoute]Guid id){
            return await _timetableService.SoftDeleteTimetableByDoctor(id, GetAccessToken());
        }
        /// <summary>
        /// Удаление записей расписания больницы
        /// </summary>
        /// <remarks>Только администраторы и менеджеры</remarks>
        [HttpDelete("Hospital/{id}")]
        [Authorize(Roles = _managerOrAdmin)]
        public async Task<IResult> DeleteTimetableByHospital([FromRoute]Guid id){
            return await _timetableService.SoftDeleteTimetableByHospital(id, GetAccessToken());
        }
        /// <summary>
        /// Получение расписания больницы по Id
        /// </summary>
        /// <remarks>Только авторизованные пользователи</remarks>
        [HttpGet("Hospital/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(List<GetTimetableByHospitalResponse>), 200)]
        [ProducesResponseType(404)]
        public async Task<IResult> GetTimetableByHospital([FromRoute]Guid id,
            [FromQuery]DateTime from, DateTime to){
            to = DateTime.SpecifyKind(to, DateTimeKind.Utc);
            from = DateTime.SpecifyKind(from, DateTimeKind.Utc);
            
            return await _timetableService.GetTimetablesByHospital(id, from, to, GetAccessToken());
        }
        /// <summary>
        /// Получение расписания врача по Id
        /// </summary>
        /// <remarks>Только авторизованные пользователи</remarks>
        [HttpGet("Doctor/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(List<GetTimetableByDoctorResponse>), 200)]
        [ProducesResponseType(404)]
        public async Task<IResult> GetTimetableByDoctor([FromRoute]Guid id,
            [FromQuery]DateTime from, DateTime to){
            to = DateTime.SpecifyKind(to, DateTimeKind.Utc);
            from = DateTime.SpecifyKind(from, DateTimeKind.Utc);
            
            return await _timetableService.GetTimetablesByDoctor(id, from, to, GetAccessToken());
        }   
        /// <summary>
        /// Получение расписания кабинета больницы
        /// </summary>
        /// <remarks>Только администраторы и менеджеры и врачи</remarks>
        [HttpGet("Hospital/{id}/Room/{room}")]
        [Authorize(Roles = _managerOrAdminOrDoctor)]
        [ProducesResponseType(typeof(List<GetTimetableByRoomResponse>), 200)]
        [ProducesResponseType(404)]
        public async Task<IResult> GetTimetableByDoctor([FromRoute]Guid id, [FromRoute] string room,
            [FromQuery]DateTime from, DateTime to){
            to = DateTime.SpecifyKind(to, DateTimeKind.Utc);
            from = DateTime.SpecifyKind(from, DateTimeKind.Utc);
            
            return await _timetableService.GetTimetablesByRoom(id, room, from, to, GetAccessToken());
        }   
        /// <summary>
        /// Получение свободных талонов на приём.
        /// </summary>
        /// <remarks>Только авторизованные пользователи</remarks>
        [HttpGet("{id}/Appointments")]
        [Authorize]
        [ProducesResponseType(typeof(List<GetAppointmentResponse>), 200)]
        [ProducesResponseType(404)]
        public async Task<IResult> GetAppointments([FromRoute]Guid id){
            return await _timetableService.GetAppointments(id);
        }   

        /// <summary>
        ///  Записаться на приём
        /// </summary>
        /// <remarks>
        /// Только авторизованные пользователи
        /// </remarks>
        [HttpPost("{id}/Appointments")]
        [Authorize]
        [ProducesResponseType(typeof(List<GetAppointmentResponse>), 200)]
        [ProducesResponseType(404)]
        public async Task<IResult> TakeAppointment([FromRoute] Guid id, [FromBody] DateTime time){
            time = DateTime.SpecifyKind(time, DateTimeKind.Utc);
            return await _timetableService.TakeAppointment(id, time, GetAccessToken());
        }

        /// <summary>
        ///  Отменить запись на приём
        /// </summary>
        /// <remarks>
        /// Только администраторы, менеджеры, и записавшийся пользователь
        /// </remarks>
        [HttpDelete("Appointments/{id}")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IResult> UntakeAppointment([FromRoute] Guid id){
            return await _timetableService.UntakeAppointment(id, GetAccessToken());
        }

        private string GetAccessToken(){
            return Request.Headers.Authorization.ToString().Replace("Bearer ", "");
        }
    }
}

