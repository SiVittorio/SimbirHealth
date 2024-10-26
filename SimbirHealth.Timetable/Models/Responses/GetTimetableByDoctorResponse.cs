using System;
using SimbirHealth.Data.SharedResponses.Hospital;

namespace SimbirHealth.Timetable.Models.Responses;

public record GetTimetableByDoctorResponse(DateTime From, DateTime To,
    List<GetAppointmentResponse> Appointments, string HospitalName, string RoomName)
    : GetTimetableResponse(From, To, Appointments);