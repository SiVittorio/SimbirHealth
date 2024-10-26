namespace SimbirHealth.Timetable.Models.Responses;

public record GetTimetableByHospitalResponse(DateTime From, DateTime To,
    List<GetAppointmentResponse> Appointments, string RoomName, string DoctorFullName)
    : GetTimetableResponse(From, To, Appointments);