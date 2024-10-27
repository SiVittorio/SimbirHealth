using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using SimbirHealth.Data.Models._Base;
using SimbirHealth.Data.Models.Account;
using SimbirHealth.Data.Models.Hospital;

namespace SimbirHealth.Data.Models.History;

/// <summary>
/// Модель истории посещения
/// </summary>
public class HistoryModel : BaseEntity, IDeleteable
{
    /// <summary>
    /// Время посещения
    /// </summary>
    public DateTime Date { get; set; }
    /// <summary>
    /// Информация
    /// </summary>
    public string Data { get; set; }
    public bool IsDeleted { get; set; }

    [ForeignKey(nameof(Pacient))]
    public Guid PacientGuid { get; set; }
    [JsonIgnore]
    public AccountModel Pacient {get; set;} 

    [ForeignKey(nameof(Doctor))]
    public Guid DoctorGuid { get; set; }
    [JsonIgnore]
    public AccountModel Doctor { get; set; }

    [ForeignKey(nameof(Hospital))]
    public Guid HospitalGuid { get; set; }
    [JsonIgnore]
    public HospitalModel Hospital{ get; set; }

    [ForeignKey(nameof(Room))]
    public Guid RoomGuid { get; set; }
    [JsonIgnore]
    public Room Room{ get; set; }
    
}
