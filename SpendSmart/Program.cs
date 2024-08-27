using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using SpendSmart.Models;
using SpendSmart.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var databaseProvider = builder.Configuration.GetValue<string>("DatabaseProvider");

if (databaseProvider == "PostgreSQL")
{
    builder.Services.AddDbContext<PostgreSqlDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection")));

    builder.Services.AddDbContext<SqlServerDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection")));

    builder.Services.AddScoped<DataSynchronizer<PostgreSqlDbContext, SqlServerDbContext>>();
    builder.Services.AddHangfire(config =>
    {
        config.UsePostgreSqlStorage(builder.Configuration.GetConnectionString("PostgreSqlConnection"));
    });
}
else if (databaseProvider == "SqlServer")
{
    builder.Services.AddDbContext<SqlServerDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection")));

    builder.Services.AddDbContext<PostgreSqlDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection")));

    builder.Services.AddScoped<DataSynchronizer<SqlServerDbContext, PostgreSqlDbContext>>();
    builder.Services.AddHangfire(config =>
    {
        config.UseSqlServerStorage(builder.Configuration.GetConnectionString("SqlServerConnection"));
    });
}

builder.Services.AddHangfireServer();

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

app.UseHangfireDashboard();

// Recurring job configuration
if (databaseProvider == "SqlServer")
{
    RecurringJob.AddOrUpdate<DataSynchronizer<SqlServerDbContext, PostgreSqlDbContext>>(
        sync => sync.SynchronizeAsync(),
        "*/1 * * * * *"); // Her saniye
}
else if (databaseProvider == "PostgreSQL")
{
    RecurringJob.AddOrUpdate<DataSynchronizer<PostgreSqlDbContext, SqlServerDbContext>>(
        sync => sync.SynchronizeAsync(),
        "*/1 * * * * *"); // Her saniye
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
