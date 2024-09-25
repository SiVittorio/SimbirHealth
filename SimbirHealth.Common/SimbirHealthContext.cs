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

        public DbSet<AccountModel> Accounts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Many to many
            modelBuilder.Entity<AccountModel>()
                .HasMany(e => e.Roles)
                .WithMany(e => e.Accounts)
                .UsingEntity<AccountToRole>();
            #endregion

            #region Seed Models
            modelBuilder.Entity<Role>().HasData(BaseDbModels.Roles);
            modelBuilder.Entity<AccountModel>().HasData(BaseDbModels.Accounts);
            modelBuilder.Entity<AccountToRole>().HasData(BaseDbModels.AccountToRoles);
            #endregion
        }
    }
}
