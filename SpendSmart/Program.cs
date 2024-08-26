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
                builder.Services.AddDbContext<ApplicationDbContext<PostgreSqlDbContext>, PostgreSqlDbContext>(options =>
                    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection")));
            }
            else if (databaseProvider == "SqlServer")
            {
                builder.Services.AddDbContext<ApplicationDbContext<SqlServerDbContext>, SqlServerDbContext>(options =>
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
