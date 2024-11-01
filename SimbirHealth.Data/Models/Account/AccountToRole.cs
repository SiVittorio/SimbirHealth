﻿using SimbirHealth.Data.Models._Base;
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
    /// Связь многие-ко-многим между таблицами Accounts и Roles
    /// </summary>
    public class AccountToRole : IDeleteable
    {
        [PrimaryKey]
        public Guid AccountGuid { get; set; }
        [PrimaryKey]
        public Guid RoleGuid { get; set; }
        public AccountModel Account { get; set; }
        public Role Role { get; set; }
        public bool IsDeleted { get; set; }
    }
}
