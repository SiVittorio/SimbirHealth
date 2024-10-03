namespace SimbirHealth.Account.Models.Responses.Account
{
    /// <summary>
    /// Ответ на получение информации о пользователе
    /// </summary>
    /// <param name="FirstName">Имя</param>
    /// <param name="LastName">Фамилия</param>
    /// <param name="Username">Никнейм пользователя</param>
    public record MeResponse(string FirstName, string LastName, string Username);
}
