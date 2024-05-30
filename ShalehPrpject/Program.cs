using Microsoft.EntityFrameworkCore;
using ShalehPrpject.Models;

namespace ShalehPrpject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var connectionString = builder.Configuration
                .GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException("No connection string found");



            // Add logging
            builder.Logging.AddConsole();
            var logger = LoggerFactory.Create(config =>
            {
                config.AddConsole();
            }).CreateLogger<Program>();
            logger.LogInformation("Using connection string: {ConnectionString}", connectionString);

            builder.Services.AddDbContext<Context>(options =>
                options.UseSqlServer(connectionString));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Flora}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
