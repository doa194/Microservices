using Microsoft.EntityFrameworkCore;

namespace Coordinator.Model.Context
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        public DbSet<Services> Services { get; set; }
        public DbSet<ServiceStatus> ServiceStatus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Services>().HasData(
                new Services { ServiceId = Guid.NewGuid(), ServiceName = "Order-Service" },
                new Services { ServiceId = Guid.NewGuid(), ServiceName = "Payment-Service" },
                new Services { ServiceId = Guid.NewGuid(), ServiceName = "Stock-Service" }
            );
        }
    }
}
