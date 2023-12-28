using Commons.Communications.Mcps;
using Commons.HubHandlers;
using Commons.Models;
using Commons.RequestStatuses;
using Hubs;
using Microsoft.AspNetCore.SignalR;
using Repositories.Managers;

namespace Services.Mcps;

public class McpFillLevelService : IMcpFillLevelService, IHostedService, IDisposable
{
    private readonly IServiceProvider _serviceProvider;

    public static Dictionary<int, float> FillLevelsById => _fillLevelsById;
    private static readonly Dictionary<int, float> _fillLevelsById = new();

    private Timer? _broadcastTimer;

    public McpFillLevelService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ParamRequestResult<GetFillLevelResponse> GetFillLevel(GetFillLevelRequest request)
    {
        var fillLevelsById = request.McpIds.ToDictionary(mcpId => mcpId, mcpId => _fillLevelsById[mcpId]);

        return new ParamRequestResult<GetFillLevelResponse>(new Success(), new GetFillLevelResponse
        {
            FillLevelsById = fillLevelsById,
        });
    }

    public ParamRequestResult<GetFillLevelResponse> GetAllFillLevel()
    {
        return new ParamRequestResult<GetFillLevelResponse>(new Success(), new GetFillLevelResponse
        {
            FillLevelsById = new Dictionary<int, float>(_fillLevelsById),
        });
    }

    public RequestResult SetFillLevel(SetFillLevelRequest request)
    {
        FillLevelsById[request.McpId] = Math.Clamp(request.FillLevel, 0f, 1f);

        using var scope = _serviceProvider.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var mcpFillLevelLog = new McpFillLevelLog
        {
            McpDataId = request.McpId,
            McpFillLevel = _fillLevelsById[request.McpId],
            Timestamp = DateTime.UtcNow,
        };
        unitOfWork.McpFillLevelLogRepository.Add(mcpFillLevelLog);
        unitOfWork.Complete();

        Console.WriteLine($"Set fill level of mcp {request.McpId} to {request.FillLevel}");

        return new RequestResult(new Success());
    }

    public RequestResult EmptyMcp(EmptyMcpRequest request)
    {
        _fillLevelsById[request.McpId] = 0f;

        using var scope = _serviceProvider.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var mcpFillLevelLog = new McpFillLevelLog
        {
            McpDataId = request.McpId,
            McpFillLevel = _fillLevelsById[request.McpId],
            Timestamp = DateTime.UtcNow,
        };
        unitOfWork.McpFillLevelLogRepository.Add(mcpFillLevelLog);

        var mcpEmptyRecord = new McpEmptyRecord
        {
            McpDataId = request.McpId,
            Timestamp = DateTime.UtcNow,
            EmptyingWorkerId = request.WorkerId,
        };
        unitOfWork.McpEmptyRecordRecordRepository.Add(mcpEmptyRecord);

        unitOfWork.Complete();

        Console.WriteLine($"Emptied mcp {request.McpId}");

        return new RequestResult(new Success());
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        InitializeFillLevelDictionary();
        _broadcastTimer = new Timer(BroadcastFillLevels, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        return Task.CompletedTask;
    }

    private void InitializeFillLevelDictionary()
    {
        using var scope = _serviceProvider.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        foreach (var mcpData in unitOfWork.McpDataRepository.GetAll())
        {
            _fillLevelsById.Add(mcpData.Id, Random.Shared.NextSingle());
        }
    }

    private void BroadcastFillLevels(object? state)
    {
        using var scope = _serviceProvider.CreateScope();
        var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<BaseHub>>();
        hubContext.Clients.All.SendAsync(HubHandlers.McpFillLevel.BROADCAST_FILL_LEVEL, new McpFillLevelBroadcastData()
        {
            FillLevelsById = _fillLevelsById,
        });
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _broadcastTimer?.Dispose();
    }
}