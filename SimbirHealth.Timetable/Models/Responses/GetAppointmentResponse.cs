using System;

namespace SimbirHealth.Timetable.Models.Responses;

/// <summary>
/// Ответ на запрос получения талонов расписания
/// </summary>
/// <param name="Guid">Идентификатор</param>
/// <param name="Time">Время приема</param>
/// <param name="IsTaken">Занято ли это время</param>
public record GetAppointmentResponse(Guid Guid, DateTime Time, bool IsTaken);
