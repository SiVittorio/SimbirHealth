namespace SimbirHealth.Account.Models.Responses.Doctor
{
    /// <summary>
    /// Ответ на запрос получения списка врачей
    /// </summary>
    /// <param name="FirstName">Имя врача</param>
    /// <param name="LastName">Фамилия врача</param>
    public record DoctorResponse(string FirstName, string LastName);
}
