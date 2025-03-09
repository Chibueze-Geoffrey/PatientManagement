using Microsoft.EntityFrameworkCore;
using PatientManagement.Application.Interface;
using PatientManagement.Application.Services;
using PatientManagement.Application.Utilities.Mapping;
using PatientManagement.Infrastructure.Data;
using PatientManagement.Infrastructure.Implementations;
using PatientManagement.Infrastructure.Interface;
using Serilog;

namespace PatientManagement.Api.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureSerilog(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((context, config) =>
            {
                config.ReadFrom.Configuration(context.Configuration);
                config.WriteTo.Console();
            });
        }

        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            // Explicitly register the AutoMapperProfile
            services.AddAutoMapper(typeof(AutoMapperProfile));
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            // Register application services
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddSingleton<ILogService, LogService>();
        }

        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            // Retrieve the connection string from the configuration
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Configure the database context
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString), ServiceLifetime.Scoped);
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            // Configure Swagger for API documentation
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        public static void ConfigureControllers(this IServiceCollection services)
        {
            // Add controllers
            services.AddControllers();
        }
    }
}