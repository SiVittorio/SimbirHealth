namespace SimbirHealth.Account.Models.Requests
{
    /// <summary>
    /// Запрос на создание пользователя
    /// </summary>
    public record SignUpRequest(string LastName,
        string FirstName, string Username, string Password);
}
