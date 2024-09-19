using Microsoft.EntityFrameworkCore;
using SimbirHealth.Data.Models.Account;

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
            var baseAccounts = new List<Role>()
            {
                new Role() { RoleName = PossibleRoles.Admin },
                new Role() { RoleName = PossibleRoles.Manager },
                new Role() { RoleName = PossibleRoles.Doctor },
                new Role() { RoleName = PossibleRoles.User }
            };

            modelBuilder.Entity<Role>().HasData(baseAccounts);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Roles)
                .WithMany(e => e.Accounts)
                .UsingEntity<AccountToRole>();

            // TODO: Добавить модели по умолчанию OnModelCreating()
            /*modelBuilder.Entity<Account>().HasData(
                new Account() { F*/
        }
    }
}
