using System.Text;
using Commons.Types;
using Helpers;
using Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repositories.Managers;
using Services.Authentication;
using Services.Map;
using Services.Mcps;
using Services.Messaging;
using Services.Reports;
using Services.Settings;
using Services.Status;
using Services.Tasks;
using Services.UserProfiles;
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

var corsPolicy = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicy,
        policyBuilder =>
        {
            policyBuilder.WithOrigins("http://localhost:47669/",
                    "https://localhost:44394/",
                    "https://urban-waste-collection.azurewebsites.net")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();
        });
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

builder.Services.AddScoped<IUserProfileService, UserProfileService>();

builder.Services.AddScoped<IMessagingService, MessagingService>();

builder.Services.AddScoped<IMcpDataService, McpDataService>();

builder.Services.AddSingleton<OnlineStatusService>();
builder.Services.AddSingleton<IOnlineStatusService>(provider => provider.GetRequiredService<OnlineStatusService>());
builder.Services.AddHostedService<OnlineStatusService>(provider => provider.GetRequiredService<OnlineStatusService>());

builder.Services.AddScoped<IWorkingStatusService, WorkingStatusService>();

builder.Services.AddSingleton<McpFillLevelService>();
builder.Services.AddSingleton<IMcpFillLevelService>(provider => provider.GetRequiredService<McpFillLevelService>());
builder.Services.AddHostedService<McpFillLevelService>(provider => provider.GetRequiredService<McpFillLevelService>());

builder.Services.AddSingleton<LocationService>();
builder.Services.AddSingleton<ILocationService>(provider => provider.GetRequiredService<LocationService>());
builder.Services.AddHostedService<LocationService>(provider => provider.GetRequiredService<LocationService>());

builder.Services.AddScoped<IDirectionService, DirectionService>();

builder.Services.AddScoped<IVehicleDataService, VehicleDataService>();

builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ITaskOptimizationService, TaskOptimizationService>();
builder.Services.AddHostedService<TaskAutoAssignmentService>();

builder.Services.AddScoped<IReportService, ReportService>();

builder.Services.AddScoped<ISettingService, SettingService>();

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

app.UseCors(corsPolicy);

app.MapControllers();

// app.SeedData();

app.Run();