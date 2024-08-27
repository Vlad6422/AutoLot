using AutoLot.Dal.EfStructures;
using AutoLot.Dal.Repos.Interfaces;
using AutoLot.Dal.Repos;
using Microsoft.EntityFrameworkCore;
using AutoLot.Dal.Initialization;
using AutoLot.Services.Logging;

namespace AutoLot.Mvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Apply the Serilog configuration using the extension method
            builder.Host.ConfigureSerilog();
            // Read configuration from appsettings.json
            var configuration = builder.Configuration;

            // Add Serilog to the builder
            // builder.Host.UseSerilog();

            builder.Services.AddScoped(typeof(IAppLogging<>), typeof(AppLogging<>));

            var connectionString = builder.Configuration.GetConnectionString("AutoLot");
            builder.Services.AddDbContextPool<ApplicationDbContext>(
                options => options.UseSqlServer(connectionString,
                sqlOptions => sqlOptions.EnableRetryOnFailure()));

            builder.Services.AddScoped<ICarRepo, CarRepo>();
            builder.Services.AddScoped<IMakeRepo, MakeRepo>();
            builder.Services.AddScoped<IOrderRepo, OrderRepo>();
            builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();
            builder.Services.AddScoped<ICreditRiskRepo, CreditRiskRepo>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                if (builder.Configuration.GetValue<bool>("RebuildDataBase"))
                {
                    SampleDataInitializer.InitializeData(new ApplicationDbContext(
                                               new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(connectionString).Options));
                }
            }
            else
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
