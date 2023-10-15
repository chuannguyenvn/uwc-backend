using Commons.Communications.Map;
using Commons.HubHandlers;
using Commons.RequestStatuses;
using Commons.Types;
using Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Services.Map;

public class LocationService : ILocationService, IAsyncDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<int, Coordinate> _locationByIds = new();

    private const int REFRESH_INTERVAL = 5;
    private Timer _locationBroadcastTimer;

    public LocationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public RequestResult UpdateLocation(LocationUpdateRequest request)
    {
        _locationByIds[request.AccountId] = request.NewLocation;
        return new RequestResult(new Success());
    }

    public void Dispose()
    {
        _locationBroadcastTimer.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _locationBroadcastTimer.DisposeAsync();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _locationBroadcastTimer = new Timer(_ => BroadcastLocation(), null, 0, REFRESH_INTERVAL);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private void BroadcastLocation()
    {
        using var scope = _serviceProvider.CreateScope();
        var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<BaseHub>>();
        hubContext.Clients.All.SendAsync(HubHandlers.Location.BROADCAST_LOCATION, new LocationBroadcastData()
        {
            LocationByIds = _locationByIds,
        });
    }
    
    private async void MockDrivingBehavior()
    {
        
    }
}