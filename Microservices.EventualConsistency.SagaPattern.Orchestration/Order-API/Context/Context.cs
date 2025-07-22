using Microsoft.EntityFrameworkCore;

namespace Order_API.Context
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }
        public DbSet<Models.Order> Orders { get; set; }
        public DbSet<Models.OrderDetails> OrderDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
