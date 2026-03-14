using Pasteleria.Data;
using Pasteleria.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Pasteleria.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register Exception Handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Agrega el contexto de base de datos utilizando ContextFactory
builder.Services.AddDbContext(builder.Configuration);

// Agrega los servicios personalizados de repositorios y servicios
builder.Services.AddCustomServices();

// Agrega los servicios personalizados de autenticaci�n y correo electr�nico
builder.Services.AddCustomAuthenticationAndEmailServices(builder.Configuration);

builder.Services.AddIdentity<User, IdentityRole>(
options =>
        options.SignIn.RequireConfirmedAccount = false)
        .AddEntityFrameworkStores<Context>()
        .AddDefaultTokenProviders();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
