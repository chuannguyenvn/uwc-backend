using MockApplication.McpFilling;
using MockApplication.TaskAssigning;
using MockApplication.WorkerActivity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHostedService<McpFillingMock>();
builder.Services.AddHostedService<TaskAssigningMock>();
builder.Services.AddHostedService<WorkerActivityMock>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();