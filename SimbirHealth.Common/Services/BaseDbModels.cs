using SimbirHealth.Data.Models.Account;
using SimbirHealth.Data.Models.Account.NotDbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimbirHealth.Common.Services
{
    /// <summary>
    /// Данные, добавляемые при создании БД
    /// </summary>
    internal static class BaseDbModels
    {
        public static List<Role> Roles = [
                new() { Guid = Guid.Parse("816dca08-d141-4fd1-8f34-7d7a4322a53d"),RoleName = PossibleRoles.Admin, DateCreate = new DateTime(2024, 9, 20, 0, 0, 0, DateTimeKind.Utc)},
                new() { Guid = Guid.Parse("ac5328b1-acec-4739-b570-90bf511a3e02"),RoleName = PossibleRoles.Manager, DateCreate = new DateTime(2024, 9, 20, 0, 0, 0, DateTimeKind.Utc) },
                new() { Guid = Guid.Parse("929a852e-4d8e-4595-9fee-00076e7a8a7b"),RoleName = PossibleRoles.Doctor, DateCreate = new DateTime(2024, 9, 20, 0, 0, 0, DateTimeKind.Utc) },
                new() { Guid = Guid.Parse("803f5318-c437-47ce-8781-97719a4095ba"),RoleName = PossibleRoles.User, DateCreate = new DateTime(2024, 9, 20, 0, 0, 0, DateTimeKind.Utc) }
            ];

        public static List<AccountModel> Accounts = [
                new(Guid.Parse("9dd5f073-265b-4b57-8268-d0a53355b7e7"), "default", "admin", "admin", Hasher.Hash("admin"), new DateTime(2024, 9, 20, 0, 0, 0, DateTimeKind.Utc)),
                new(Guid.Parse("dfa3ea95-21e1-44c6-9393-5ab531d39acd"), "default", "manager", "manager", Hasher.Hash("manager"),  new DateTime(2024, 9, 20, 0, 0, 0, DateTimeKind.Utc)),
                new(Guid.Parse("2018473b-0ec4-4702-bbaf-667e4843a48a"), "default", "doctor", "doctor", Hasher.Hash("doctor"), new DateTime(2024, 9, 20, 0, 0, 0, DateTimeKind.Utc)),
                new(Guid.Parse("c6645389-3937-4d85-80e2-05437a15241b"), "default", "user", "user", Hasher.Hash("user"), new DateTime(2024, 9, 20, 0, 0, 0, DateTimeKind.Utc))
            ];

        public static List<AccountToRole> AccountToRoles = [
                new AccountToRole() { AccountGuid = Accounts[0].Guid, RoleGuid = Roles[0].Guid },
                new AccountToRole() { AccountGuid = Accounts[1].Guid, RoleGuid = Roles[1].Guid },
                new AccountToRole() { AccountGuid = Accounts[2].Guid, RoleGuid = Roles[2].Guid },
                new AccountToRole() { AccountGuid = Accounts[3].Guid, RoleGuid = Roles[3].Guid }
            ];
    }
}
