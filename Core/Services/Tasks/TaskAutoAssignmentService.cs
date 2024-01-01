namespace Services.Tasks;

public class TaskAutoAssignmentService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private Timer? _optimizationTimer;

    public TaskAutoAssignmentService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _optimizationTimer = new Timer(Optimize, null, TimeSpan.Zero, TimeSpan.FromSeconds(TaskOptimizationService.AUTO_DISTRIBUTION_INTERVAL_SECONDS));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _optimizationTimer?.Dispose();
        return Task.CompletedTask;
    }

    private void Optimize(object? state)
    {
        using var scope = _serviceProvider.CreateScope();
        var taskOptimizationService = scope.ServiceProvider.GetRequiredService<ITaskOptimizationService>();
        if (taskOptimizationService.IsAutoTaskDistributionEnabled) taskOptimizationService.DistributeTasksFromPool();
    }
}