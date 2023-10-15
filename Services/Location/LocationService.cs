using Commons.Communications.Location;
using Commons.HubHandlers;
using Commons.RequestStatuses;
using Commons.Types;
using Microsoft.AspNetCore.SignalR;
using Repositories.Managers;

namespace Services.Location;

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

    public RequestResult UpdateLocation(int accountId, LocationUpdateRequest request)
    {
        _locationByIds[accountId] = request.NewLocation;
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
        var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext>();
        hubContext.Clients.All.SendAsync(HubHandlers.Location.BROADCAST_LOCATION, new LocationBroadcastData()
        {
            LocationByIds = _locationByIds,
        });
    }
}