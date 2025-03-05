using Microsoft.EntityFrameworkCore;
using PatientManagement.Api.Extensions;
using PatientManagement.Infrastructure.Data;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog for logging
builder.ConfigureSerilog();

// Register AutoMapper
builder.Services.ConfigureAutoMapper();

// Add application services
builder.Services.ConfigureServices();

// Configure the database
builder.Services.ConfigureDatabase(builder.Configuration);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserOrAdminPolicy", policy =>
        policy.RequireRole("User", "Admin"));
});

// Add controllers and Swagger
builder.Services.ConfigureControllers();
builder.Services.ConfigureSwagger();

// Allow dynamic port from Render
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

var app = builder.Build();

// Apply database migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

// Middleware for global exception handling
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Enable Swagger and Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();