using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using FluentValidation;
using FluentValidation.AspNetCore;

using Serilog;
using System.Text;
using PatientService.API.Filters;
using PatientService.Application.Validators;
using PatientService.Core.Repositories;
using PatientService.Infrastructure.Data;
using PatientService.Infrastructure.Messaging;
using PatientService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// -----------------------------
// Configure Serilog
// -----------------------------
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/patientservice-log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();


// -----------------------------
// Register Services
// -----------------------------

// Controllers + Global Exception Filter
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("UserDb"));

// Dependency Injection
builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<IJwtService, JwtService>();

// Kafka
builder.Services.AddSingleton<KafkaProducerService>();


// -----------------------------
// JWT Authentication
// -----------------------------
var key = configuration["Jwt:Key"];

if (string.IsNullOrEmpty(key))
{
    throw new Exception("JWT Key is missing in configuration");
}

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],

            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

builder.Services.AddAuthorization();


// -----------------------------
// Swagger
// -----------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// -----------------------------
// Build Application
// -----------------------------
var app = builder.Build();


// -----------------------------
// Configure Middleware Pipeline
// -----------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Authentication MUST come before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();