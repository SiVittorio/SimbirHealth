using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace SimbirHealth.Data.Models.Account
{
    public class AccountToRole
    {
        [PrimaryKey]
        public Guid AccountGuid { get; set; }
        [PrimaryKey]
        public Guid RoleGuid { get; set; }
        public Account Account { get; set; }
        public Role Role { get; set; }
    }
}
