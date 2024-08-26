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
                // Register PostgreSqlDbContext
                builder.Services.AddDbContext<PostgreSqlDbContext>(options =>
                    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection")));

                // Register ApplicationDbContext<PostgreSqlDbContext> with correct options
                builder.Services.AddDbContext<ApplicationDbContext<PostgreSqlDbContext>>(options =>
                    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection")));
            }
            else
            {
                // Register SqlServerDbContext
                builder.Services.AddDbContext<SqlServerDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection")));

                // Register ApplicationDbContext<SqlServerDbContext> with correct options
                builder.Services.AddDbContext<ApplicationDbContext<SqlServerDbContext>>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection")));
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
