namespace MockApplication.Base;

public abstract class BaseHostedService : IHostedService
{
    protected abstract Task Main();
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await Main();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}