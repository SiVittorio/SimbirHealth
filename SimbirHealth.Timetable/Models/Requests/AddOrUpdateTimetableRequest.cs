using System;

namespace SimbirHealth.Timetable.Models.Requests;

public record AddOrUpdateTimetableRequest(Guid HospitalId, Guid DoctorId,
    DateTime From, DateTime To, string Room);
