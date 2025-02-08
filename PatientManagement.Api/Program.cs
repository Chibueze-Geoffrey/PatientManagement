using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PatientManagement.Api.MiddleWares;
using PatientManagement.Application.Interface;
using PatientManagement.Application.Services;
using PatientManagement.Infrastructure.Data;
using PatientManagement.Infrastructure.Implementations;
using PatientManagement.Infrastructure.Interface;

var builder = WebApplication.CreateBuilder(args);

// Register AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add services to the container.
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Database

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=patientmanagement.db")
           .EnableSensitiveDataLogging(), ServiceLifetime.Scoped);// Shows SQL parameters

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Allow dynamic port from Render
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

// Middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();


//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
