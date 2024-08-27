using Microsoft.EntityFrameworkCore;

namespace SpendSmart.Models
{
    public class SqlServerDbContext : ApplicationDbContext<SqlServerDbContext>
    {
        public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options)
            : base(options)
        {
        }
    }
}