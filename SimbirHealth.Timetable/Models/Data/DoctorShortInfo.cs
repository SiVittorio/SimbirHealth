namespace SimbirHealth.Timetable.Models.Data;

/// <summary>
/// Модель хранения краткой информации о враче
/// </summary>
/// <param name="FirstName"></param>
/// <param name="LastName"></param>
public record DoctorShortInfo(string FirstName, string LastName);
