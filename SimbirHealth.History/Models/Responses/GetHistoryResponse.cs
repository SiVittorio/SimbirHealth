using System;

namespace SimbirHealth.History.Models.Responses;

/// <summary>
/// Модель ответа на запрос получения данных об истории посещений
/// </summary>
/// <param name="Date">Дата посещения</param>
/// <param name="Data">Информация</param>
/// <param name="HospitalName">Название больницы</param>
/// <param name="PacientFullName">Имя пациента</param>
/// <param name="DoctorFullName">Имя врача</param>
/// <param name="RoomName">Название кабинета</param>
public record GetHistoryResponse(DateTime Date, string Data, string HospitalName, 
    string PacientFullName, string DoctorFullName, string RoomName);
