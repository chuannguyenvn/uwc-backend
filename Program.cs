using System.Text;
using Commons.Models;
using Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repositories.Managers;
using Services.Authentication;
using Services.Mcps;
using Services.Messaging;
using Services.Vehicles;

var builder = WebApplication.CreateBuilder(args);

#region Security

var settings = new Settings();
builder.Configuration.Bind("Settings", settings);
builder.Services.AddSingleton(settings);
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.BearerKey)),
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });

#endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

#region Database

builder.Services.AddDbContext<UwcDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("Database")));
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

#endregion

#region Services

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IMessagingService, MessagingService>();
builder.Services.AddScoped<IMcpDataService, McpDataService>();
builder.Services.AddSingleton<McpFillLevelService>();
builder.Services.AddSingleton<IMcpFillLevelService>(provider => provider.GetRequiredService<McpFillLevelService>());
builder.Services.AddHostedService<McpFillLevelService>(provider => provider.GetRequiredService<McpFillLevelService>());
builder.Services.AddScoped<IVehicleDataService, VehicleDataService>();

#endregion

var app = builder.Build();

#region SignalR

app.MapHub<BaseHub>("/hub");

#endregion

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

// app.ResetData();

// app.SeedData();

app.Run();