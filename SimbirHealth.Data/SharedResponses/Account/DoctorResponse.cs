using System;

namespace SimbirHealth.Data.SharedResponses.Account;

/// <summary>
/// Ответ на запрос получения списка врачей
/// </summary>
/// <param name="FirstName">Имя врача</param>
/// <param name="LastName">Фамилия врача</param>
public record DoctorResponse(Guid Guid, string FirstName, string LastName);
