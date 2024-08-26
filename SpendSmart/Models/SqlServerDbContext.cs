using Microsoft.EntityFrameworkCore;

namespace SpendSmart.Models
{
    public class SqlServerDbContext : ApplicationDbContext<SqlServerDbContext>
    {
        public SqlServerDbContext(DbContextOptions<ApplicationDbContext<SqlServerDbContext>> options)
            : base(options)
        {
        }
    }
}