namespace SimbirHealth.Account.Models.Requests
{
    /// <summary>
    /// Запрос на вход пользователя в систему
    /// </summary>
    public record SignInRequest(string Username, string Password);
}
