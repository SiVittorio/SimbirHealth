using System;

namespace SimbirHealth.Timetable.Models.Requests;

public class AddOrUpdateTimetableRequest{
    public AddOrUpdateTimetableRequest(Guid hospitalId,
        Guid doctorId,
        DateTime from,
        DateTime to, 
        string room)
    {
        HospitalId = hospitalId;
        DoctorId = doctorId;
        From = DateTime.SpecifyKind(from, DateTimeKind.Utc);
        To = DateTime.SpecifyKind(to, DateTimeKind.Utc);
        Room = room;
    }
    public Guid HospitalId { get; }
    public Guid DoctorId { get; }
    public DateTime From { get; }
    public DateTime To { get; }
    public string Room { get; }
}
