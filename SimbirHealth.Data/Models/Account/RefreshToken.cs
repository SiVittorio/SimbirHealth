using SimbirHealth.Data.Models._Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
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
        public RefreshToken(string Username)
        {
            Token = string.Format("{0}{1}", Guid.NewGuid().ToString(), Username);
            DateCreate = DateTime.UtcNow;
        }

        /// <summary>
        /// Имя владельца токена обновления
        /// </summary>
        [Key]
        public string Token { get; set; }
        /// <summary>
        /// Дата создания токена
        /// </summary>
        public DateTime DateCreate { get; set; }
    }
}
