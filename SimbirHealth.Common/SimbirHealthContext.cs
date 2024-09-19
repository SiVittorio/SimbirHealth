using Microsoft.EntityFrameworkCore;

namespace SimbirHealth.Common
{
    public class SimbirHealthContext : DbContext
    {
        public SimbirHealthContext(DbContextOptions<SimbirHealthContext> options) : base(options)
        {
            Database.EnsureCreated(); // create if not exist
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TODO: Добавить модели по умолчанию OnModelCreating()
            base.OnModelCreating(modelBuilder);
        }
    }
}
