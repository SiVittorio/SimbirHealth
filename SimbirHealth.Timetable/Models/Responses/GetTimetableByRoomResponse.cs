using System;

namespace SimbirHealth.Timetable.Models.Responses;

public record GetTimetableByRoomResponse(DateTime From, DateTime To,
    List<GetAppointmentResponse> Appointments, string DoctorFullName)
    : GetTimetableResponse(From, To, Appointments);
