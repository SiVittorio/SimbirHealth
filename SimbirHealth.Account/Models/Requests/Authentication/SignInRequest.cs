namespace SimbirHealth.Account.Models.Requests.Authentication
{
    /// <summary>
    /// Запрос на вход пользователя в систему
    /// </summary>
    public record SignInRequest(string Username, string Password);
}
