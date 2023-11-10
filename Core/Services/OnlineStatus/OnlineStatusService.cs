using Commons.HubHandlers;
using Hubs;
using Microsoft.AspNetCore.SignalR;
using SharedLibrary.Communications.OnlineStatus;

namespace Services.OnlineStatus;

public class OnlineStatusService : IOnlineStatusService
{
    private const int UPDATE_INTERVAL_SECONDS = 5;
    
    private readonly IServiceProvider _serviceProvider;
    private Timer? _timer;

    public OnlineStatusService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(UpdateAccountStatus, null, TimeSpan.Zero, TimeSpan.FromSeconds(UPDATE_INTERVAL_SECONDS));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Dispose();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    private void UpdateAccountStatus(object? state)
    {
        using var scope = _serviceProvider.CreateScope();
        var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<BaseHub>>();
        hubContext.Clients.All.SendAsync(HubHandlers.OnlineStatus.UPDATE_STATUSES, new OnlineStatusBroadcastData()
        {
            OnlineAccountIds = BaseHub.ConnectionIds.Keys.ToList(),
        });
    }
}