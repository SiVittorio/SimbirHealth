namespace SimbirHealth.Account.Models.Requests.Account
{
    /// <summary>
    /// Запрос на обновление информации о пользоватле 
    /// </summary>
    /// <param name="LastName">Фамилия</param>
    /// <param name="FirstName">Имя</param>
    /// <param name="Password">Пароль</param>
    public record UpdateRequest(string LastName, string FirstName, string Password);
}
