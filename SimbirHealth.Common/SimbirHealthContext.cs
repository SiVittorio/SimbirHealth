using Microsoft.EntityFrameworkCore;
using SimbirHealth.Common.Services.Db;
using SimbirHealth.Data.Models.Account;
using SimbirHealth.Data.Models.Hospital;
using SimbirHealth.Data.Models.Timetable;
using System.Security.Cryptography;

namespace SimbirHealth.Common
{
    public class SimbirHealthContext : DbContext
    {
        public SimbirHealthContext(DbContextOptions<SimbirHealthContext> options) : base(options)
        {
            Database.EnsureCreated(); // create if not exist
        }

        #region Account
        public DbSet<AccountModel> Accounts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        #endregion

        #region Hospital
        public DbSet<HospitalModel> Hospitals { get; set; }
        public DbSet<Room> Rooms { get; set; }
        #endregion

        #region Timetable
        public DbSet<TimetableModel> Timetables { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        #endregion


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Many to many
            modelBuilder.Entity<AccountModel>()
                .HasMany(e => e.Roles)
                .WithMany(e => e.Accounts)
                .UsingEntity<AccountToRole>();
            #endregion
            #region One to one
            modelBuilder.Entity<AccountModel>()
                .HasOne(e => e.RefreshToken)
                .WithOne(e => e.Account)
                .HasForeignKey<RefreshToken>(e => e.AccountGuid);
            #endregion

            #region Seed Models
            modelBuilder.Entity<Role>().HasData(BaseDbModels.Roles);
            modelBuilder.Entity<AccountModel>().HasData(BaseDbModels.Accounts);
            modelBuilder.Entity<AccountToRole>().HasData(BaseDbModels.AccountToRoles);
            #endregion
        }
    }
}
