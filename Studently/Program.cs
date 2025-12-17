using Studently.Common.Database;
using Studently.Authentication.Service;
using Studently.User.Repository;
using Studently.User.Service;
using Studently.UserCredential.Repository;
using Studently.UserCredential.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register database connection factory
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

// Register services
builder.Services.AddSingleton<IPasswordHashingStrategy, SHA512PasswordHashingStrategy>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserCredentialService, UserCredentialService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

// Register repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserCredentialRepository, UserCredentialRepository>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
