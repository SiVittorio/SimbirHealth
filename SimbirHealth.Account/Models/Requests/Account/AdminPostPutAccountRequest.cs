using SimbirHealth.Data.Models.Account;

namespace SimbirHealth.Account.Models.Requests.Account
{
    /// <summary>
    /// Запрос на добавление/обновление аккаунта администратором
    /// </summary>
    /// <param name="LastName">Фамилия</param>
    /// <param name="FirstName">Имя</param>
    /// <param name="Username">Имя пользователя</param>
    /// <param name="Password">Пароль</param>
    /// <param name="Roles">Роли</param>
    public record AdminPostPutAccountRequest(
        string LastName,
        string FirstName,
        string Username,
        string Password,
        List<string> Roles
        );
}
