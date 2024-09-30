using SimbirHealth.Data.Models._Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace SimbirHealth.Data.Models.Account
{
    /// <summary>
    /// Модель токена обновления для JWT 
    /// </summary>
    public class RefreshToken
    {
        public RefreshToken() { }
        public RefreshToken(string Username, DateTime expiredDate, Guid accountGuid)
        {
            Token = string.Format("{0}{1}", Guid.NewGuid().ToString(), Username);
            ExpiredDate = expiredDate;
            AccountGuid = accountGuid;
        }

        /// <summary>
        /// Имя владельца токена обновления
        /// </summary>
        [Key]
        public string Token { get; set; }
        /// <summary>
        /// Срок годности токена
        /// </summary>
        [Required]
        public DateTime ExpiredDate { get; set; }
        /// <summary>
        /// Аккаунт-владелец токена
        /// </summary>
        [JsonIgnore]
        [Required]
        [IndexColumn(IsUnique = true)]
        public AccountModel Account{ get; set; }
        [ForeignKey(nameof(Account))]
        [JsonIgnore]
        [IndexColumn(IsUnique = true)]
        public Guid AccountGuid { get; set; }
    }
}
