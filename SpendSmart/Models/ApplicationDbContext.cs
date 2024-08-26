using Microsoft.EntityFrameworkCore;

namespace SpendSmart.Models
{
    public class ApplicationDbContext<TContext> : DbContext where TContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext<TContext>> options)
            : base(options)
        {
        }

        public DbSet<Expense> Expenses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Expense>()
                .Property(e => e.Value)
                .HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);
        }
    }
}
