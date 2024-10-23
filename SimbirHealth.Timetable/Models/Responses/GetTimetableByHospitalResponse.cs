using System;
using SimbirHealth.Data.SharedResponses.Account;
using SimbirHealth.Data.SharedResponses.Hospital;
using SimbirHealth.Timetable.Models.Data;

namespace SimbirHealth.Timetable.Models.Responses;

public record GetTimetableByHospitalResponse(DateTime From, DateTime To,
    List<GetAppointmentResponse> Appointments, string RoomName, DoctorShortInfo DoctorInfo)
    : GetTimetableResponse(From, To, Appointments);