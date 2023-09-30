using Commons.Communications.Mcps;
using Commons.RequestStatuses;
using Repositories.Managers;

namespace Services.Mcps;

public class McpFillLevelService : IMcpFillLevelService, IHostedService, IDisposable
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<int, float> _fillLevelsById = new();
    private Timer? _databasePersistTimer;
    private Timer? _fillTimer;

    public McpFillLevelService(IUnitOfWork unitOfWork, IServiceProvider serviceProvider)
    {
        _unitOfWork = unitOfWork;
        _serviceProvider = serviceProvider;
    }

    public ParamRequestResult<GetFillLevelResponse> GetAllFillLevel()
    {
        return new ParamRequestResult<GetFillLevelResponse>(new Success(), new GetFillLevelResponse
        {
            FillLevelsById = new Dictionary<int, float>(_fillLevelsById),
        });
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        InitializeFillLevelDictionary();

        _fillTimer = new Timer(FillMcps, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        // _databasePersistTimer = new Timer(PersistMcpStates, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));

        return Task.CompletedTask;
    }

    private void FillMcps(object? state)
    {
        foreach (var (id, _) in _fillLevelsById)
        {
            _fillLevelsById[id] += (float)new Random().NextDouble();
        }
    }

    private void InitializeFillLevelDictionary()
    {
        using var scope = _serviceProvider.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<UnitOfWork>();
        foreach (var mcpData in unitOfWork.McpData.GetAll())
        {
            _fillLevelsById.Add(mcpData.Id, 0f);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _fillTimer?.Dispose();
        _databasePersistTimer?.Dispose();
    }
}