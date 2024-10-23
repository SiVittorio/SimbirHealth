using System;

namespace SimbirHealth.Timetable.Models.Responses;

/// <summary>
/// Ответ на запрос получения списка расписаний
/// </summary>
/// <param name="From">Начало расписания</param>
/// <param name="To">Конец расписания</param>
/// <param name="Appointments">Талоны для записи</param>
public record GetTimetableResponse(DateTime From, DateTime To,
    List<GetAppointmentResponse> Appointments);
