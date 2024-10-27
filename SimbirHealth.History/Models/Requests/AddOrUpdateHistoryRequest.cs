using System;

namespace SimbirHealth.History.Models.Requests;

/// <summary>
/// Модель запроса на создание или обновление
/// истории посещений 
/// </summary>
public class AddOrUpdateHistoryRequest{
    public AddOrUpdateHistoryRequest(DateTime date,
        Guid pacientId, Guid hospitalId,
        Guid doctorId, string roomName,
        string data)
    {
        Date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
        PacientId = pacientId;
        HospitalId = hospitalId;
        DoctorId = doctorId;
        RoomName = roomName;
        Data = data;
    }

    public DateTime Date { get; }
    public Guid PacientId { get; }
    public Guid HospitalId { get; }
    public Guid DoctorId { get; }
    public string RoomName { get; }
    public string Data { get; }
}
