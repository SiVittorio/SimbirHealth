using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimbirHealth.Data.Models.Account
{
    public class AccountToRole
    {
        [Key]
        public Guid AccountGuid { get; set; }
        [Key]
        public Guid RoleGuid { get; set; }
        public Account Account { get; set; }
        public Role Role { get; set; }
    }
}
