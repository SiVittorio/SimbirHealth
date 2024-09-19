using SimbirHealth.Data.Models._Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace SimbirHealth.Data.Models.Account
{
    /// <summary>
    /// Роль пользователя
    /// </summary>
    public class Role : BaseEntity
    {
        /// <summary>
        /// Название роли
        /// </summary>
        [Required]
        [IndexColumn(IsUnique = true)]
        public string RoleName { get; set; }

        [JsonIgnore]
        public List<Account>? Accounts { get; set; }
        [JsonIgnore]
        public List<AccountToRole>? AccountToRoles { get; set; }
    }
}
