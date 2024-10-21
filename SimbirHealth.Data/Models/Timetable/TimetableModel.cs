using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using SimbirHealth.Data.Models._Base;
using SimbirHealth.Data.Models.Account;
using SimbirHealth.Data.Models.Hospital;

namespace SimbirHealth.Data.Models.Timetable;
/// <summary>
/// Модель расписания
/// </summary>
public class TimetableModel : BaseEntity, IDeleteable
{
    public TimetableModel(){}
    /// <summary>
    /// Начало периода
    /// </summary>
    public DateTime From { get; set; }
    /// <summary>
    /// Конец периода
    /// </summary>
    public DateTime To { get; set; }
    public bool IsDeleted { get; set; }

    [JsonIgnore]
    public List<Appointment> Appointments { get; set; }

    [ForeignKey(nameof(Hospital))]
    public Guid HospitalGuid { get; set; }
    [JsonIgnore]
    public HospitalModel Hospital { get; set; }

    [ForeignKey(nameof(Doctor))]
    public Guid DoctorGuid { get; set; }
    [JsonIgnore]
    public AccountModel Doctor { get; set; }

    [ForeignKey(nameof(Room))]
    public Guid RoomGuid { get; set; }
    [JsonIgnore]
    public Room Room { get; set; }
}
