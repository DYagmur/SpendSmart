using Microsoft.EntityFrameworkCore;

namespace SpendSmart.Models
{
    public class PostgreSqlDbContext : ApplicationDbContext<PostgreSqlDbContext>
    {
        public PostgreSqlDbContext(DbContextOptions<PostgreSqlDbContext> options)
            : base(options)
        {
        }
    }
}