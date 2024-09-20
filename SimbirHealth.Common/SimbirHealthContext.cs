using Microsoft.EntityFrameworkCore;
using SimbirHealth.Common.Services;
using SimbirHealth.Data.Models.Account;
using System.Security.Cryptography;

namespace SimbirHealth.Common
{
    public class SimbirHealthContext : DbContext
    {
        public SimbirHealthContext(DbContextOptions<SimbirHealthContext> options) : base(options)
        {
            Database.EnsureCreated(); // create if not exist
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Роли по умолчанию
            var baseRoles = new List<Role>()
            {
                new() { RoleName = PossibleRoles.Admin },
                new() { RoleName = PossibleRoles.Manager },
                new() { RoleName = PossibleRoles.Doctor },
                new() { RoleName = PossibleRoles.User }
            };

            var baseAccounts = new List<Account>()
            {
                new("default", "admin", "admin", Hasher.Hash("admin"), [baseRoles[0]]),
                new("default", "manager", "manager", Hasher.Hash("manager"), [baseRoles[1]]),
                new("default", "doctor", "doctor", Hasher.Hash("doctor"), [baseRoles[2]]),
                new("default", "user", "user", Hasher.Hash("user"), [baseRoles[3]])
            };

            modelBuilder.Entity<Role>().HasData(baseRoles);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Roles)
                .WithMany(e => e.Accounts)
                .UsingEntity<AccountToRole>();

            // Аккаунты по умолчанию 
            modelBuilder.Entity<Account>().HasData(baseAccounts);

            modelBuilder.Entity<AccountToRole>().HasData(
                new AccountToRole() { AccountGuid = baseAccounts[0].Guid, RoleGuid = baseRoles[0].Guid },
                new AccountToRole() { AccountGuid = baseAccounts[1].Guid, RoleGuid = baseRoles[1].Guid },
                new AccountToRole() { AccountGuid = baseAccounts[2].Guid, RoleGuid = baseRoles[2].Guid },
                new AccountToRole() { AccountGuid = baseAccounts[3].Guid, RoleGuid = baseRoles[3].Guid });
        }
    }
}
