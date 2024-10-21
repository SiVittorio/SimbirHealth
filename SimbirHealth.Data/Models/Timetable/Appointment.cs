using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using SimbirHealth.Data.Models._Base;

namespace SimbirHealth.Data.Models.Timetable;

/// <summary>
/// Модель талона на прием
/// </summary>
public class Appointment : BaseEntity, IDeleteable
{
    public Appointment(){}

    /// <summary>
    /// Время талона
    /// </summary>
    public DateTime Time { get; set; }
    /// <summary>
    /// Занят ли талон
    /// </summary>
    public bool IsTaken { get; set; }
    public bool IsDeleted { get; set; }


    [ForeignKey(nameof(Timetable))]
    public Guid TimetableGuid { get; set; }
    [JsonIgnore]
    public TimetableModel Timetable { get; set; }
}