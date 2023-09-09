using Controllers;
using Repositories;
using Services.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UwcDbContext>();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();