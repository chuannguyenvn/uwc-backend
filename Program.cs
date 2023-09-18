using System.Net;
using Helpers;
using Hubs;
using Microsoft.EntityFrameworkCore;
using Repositories.Managers;
using Services.Authentication;
using Services.Mcps;
using Services.Messaging;
using Services.Vehicles;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<UwcDbContext>(options => options.UseSqlite("Data Source=MyDatabase.sqlite;"));
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IMessagingService, MessagingService>();
builder.Services.AddScoped<IMcpDataService, McpDataService>();
builder.Services.AddScoped<IVehicleDataService, VehicleDataService>();

builder.Services.AddSignalR();

var app = builder.Build();

app.MapHub<MessagingHub>("/chat");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("./swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.SeedData();

app.Run();