using SimbirHealth.Data.Models._Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

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

        public AccountModel(Guid guid, string lastName, string firstName, string username, string password, DateTime dateCreate)
        {
            Guid = guid;
            LastName = lastName;
            FirstName = firstName;
            Username = username;
            Password = password;
            DateCreate = dateCreate;
        }

        public AccountModel(string lastName, string firstName, string username, string password)
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
        [IndexColumn(IsUnique = true)]
        public string Username { get; set; }

        /// <summary>
        /// Пароль пользователя аккаунта
        /// </summary>
        public string Password { get; set; }

        [JsonIgnore]
        public List<Role>? Roles { get; set; }
        [JsonIgnore]
        public List<AccountToRole>? AccountToRoles { get; set; }

        [JsonIgnore]
        [IndexColumn(IsUnique = true)]
        public RefreshToken? RefreshToken { get; set; }
    }
}
