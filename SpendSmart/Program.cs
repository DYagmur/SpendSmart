using Microsoft.EntityFrameworkCore;
using SpendSmart.Models;

namespace SpendSmart
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            var databaseProvider = builder.Configuration.GetValue<string>("DatabaseProvider");

            if (databaseProvider == "PostgreSQL")
            {
                // Create DbContextOptions for PostgreSqlDbContext
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext<PostgreSqlDbContext>>();
                optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection"));

                // Register ApplicationDbContext<PostgreSqlDbContext>
                builder.Services.AddScoped(sp =>
                    new ApplicationDbContext<PostgreSqlDbContext>(optionsBuilder.Options));

                // Register PostgreSqlDbContext
                builder.Services.AddScoped(sp =>
                    new PostgreSqlDbContext(sp.GetRequiredService<DbContextOptions<ApplicationDbContext<PostgreSqlDbContext>>>()));
            }
            else
            {
                // Create DbContextOptions for SqlServerDbContext
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext<SqlServerDbContext>>();
                optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"));

                // Register ApplicationDbContext<SqlServerDbContext>
                builder.Services.AddScoped(sp =>
                    new ApplicationDbContext<SqlServerDbContext>(optionsBuilder.Options));

                // Register SqlServerDbContext
                builder.Services.AddScoped(sp =>
                    new SqlServerDbContext(sp.GetRequiredService<DbContextOptions<ApplicationDbContext<SqlServerDbContext>>>()));
            }

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
