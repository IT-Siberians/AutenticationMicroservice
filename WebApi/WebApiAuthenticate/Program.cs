using EntityFramework;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PasswordHasher;
using Repositories.Abstractions;
using Repositories.Implementations.EntityFrameworkRepositories;
using Services.Abstractions;
using Services.Implementations;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Enums;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using WebApiAuthenticate.Extensions;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;
var userDbConString = configuration.GetConnectionString("UsersDb");
if (string.IsNullOrWhiteSpace(userDbConString))
    throw new InvalidOperationException("The connection string 'UsersDb' cannot be null or empty.");

// Add DbContext to the container.
services.AddDbContext<UserDbContext>(options => options.UseNpgsql(userDbConString,
    opt => opt.MigrationsAssembly("EntityFramework")));

// Add repositories to the container.
services.AddScoped<IUserRepository, UserRepository>();

// Add services to the container.
services.AddTransient<IUserManagementService, UserManagementService>();
services.AddTransient<INotificationService, NotificationService>();
services.AddTransient<IUserValidationService, UserValidationService>();

// Add infrastructure to the container.
services.AddTransient<IPasswordHasher, CustomPasswordHasher>();
services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
services.AddFluentValidationAutoValidation(configuration =>
{
    configuration.ValidationStrategy = ValidationStrategy.All;
}).AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(
c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Authenticate API",
        Description = "Authentication and registration API used for authentication, user creation and management (registration, password change, username change, email change, user deletion)"
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //c =>
    //{
    //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    //    c.RoutePrefix = string.Empty; // ������ � Swagger UI �� ��������� URL
    //});
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MigrateDatabase<UserDbContext>();

app.Run();