using AutoLot.Api.Filters;
using AutoLot.Dal.EfStructures;
using AutoLot.Dal.Initialization;
using AutoLot.Dal.Repos;
using AutoLot.Dal.Repos.Interfaces;
using AutoLot.Services.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Reflection;

namespace AutoLot.Api
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
            //builder.Host.UseSerilog();

            builder.Services.AddScoped(typeof(IAppLogging<>), typeof(AppLogging<>));

            // Add DbContext and Repositories
            var connectionString = configuration.GetConnectionString("AutoLot");
            builder.Services.AddDbContextPool<ApplicationDbContext>(
                options => options.UseSqlServer(connectionString,
                sqlOptions => sqlOptions.EnableRetryOnFailure()));

            builder.Services.AddScoped<ICarRepo, CarRepo>();
            builder.Services.AddScoped<IMakeRepo, MakeRepo>();
            builder.Services.AddScoped<IOrderRepo, OrderRepo>();
            builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();
            builder.Services.AddScoped<ICreditRiskRepo, CreditRiskRepo>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
            // Add services to the container
            builder.Services.AddControllers(config=>config.Filters.Add(new CustomExceptionFilterAttribute(builder.Environment)))
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                    options.JsonSerializerOptions.WriteIndented = true;
                });

            // Configure Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AutoLot Service", Version = "v1", Description = "Service to support the AutoLot dealer site" });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                if (configuration.GetValue<bool>("RebuildDataBase"))
                {
                    SampleDataInitializer.InitializeData(new ApplicationDbContext(
                                               new DbContextOptionsBuilder<ApplicationDbContext>()
                                                                          .UseSqlServer(connectionString).Options));
                }
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthorization();

            app.MapControllers();

            try
            {
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "The application failed to start correctly.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
