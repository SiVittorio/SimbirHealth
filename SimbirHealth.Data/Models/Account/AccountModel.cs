using SimbirHealth.Data.Models._Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SimbirHealth.Data.Models.Account
{
    /// <summary>
    /// Аккаунт
    /// </summary>
    public class AccountModel : BaseEntity
    {
        public AccountModel()
        {
        }

        public AccountModel(string lastName, string firstName, string username, string password, List<Role>? roles)
        {
            LastName = lastName;
            FirstName = firstName;
            Username = username;
            Password = password;
        }

        /// <summary>
        /// Фамилия пользователя аккаунта
        /// </summary>
        public string LastName { get; set; }
        
        /// <summary>
        /// Имя пользователя аккаунта
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Никнейм пользователя аккаунта
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Пароль пользователя аккаунта
        /// </summary>
        public string Password { get; set; }

        [JsonIgnore]
        public List<Role>? Roles { get; set; }
        [JsonIgnore]
        public List<AccountToRole>? AccountToRoles { get; set; }
    }
}
