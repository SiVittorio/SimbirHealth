using System;

namespace SimbirHealth.Timetable.Models.Responses;

/// <summary>
/// Ответ на запрос получения талонов расписания
/// </summary>
/// <param name="Time">Время приема</param>
/// <param name="IsTaken">Занято ли это время</param>
public record GetAppointmentResponse(DateTime Time, bool IsTaken);
