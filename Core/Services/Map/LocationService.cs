using Commons.Categories;
using Commons.Communications.Map;
using Commons.HubHandlers;
using Commons.RequestStatuses;
using Commons.Types;
using Hubs;
using Microsoft.AspNetCore.SignalR;
using Repositories.Managers;

namespace Services.Map;

public class LocationService : ILocationService
{
    private readonly IServiceProvider _serviceProvider;

    private readonly Dictionary<int, Coordinate> _driverLocationsById = new();
    public Dictionary<int, Coordinate> DriverLocationsByAccountId => _driverLocationsById;

    private readonly Dictionary<int, Coordinate> _cleanerLocationsById = new();
    public Dictionary<int, Coordinate> CleanerLocationsByAccountId => _cleanerLocationsById;

    private const int REFRESH_INTERVAL = 1000;
    private Timer? _locationBroadcastTimer;

    public LocationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ParamRequestResult<GetLocationResponse> GetLocation(GetLocationRequest request)
    {
        if (_driverLocationsById.TryGetValue(request.AccountId, out var driverLocation))
            return new ParamRequestResult<GetLocationResponse>(new Success(), new GetLocationResponse()
            {
                Coordinate = driverLocation
            });

        if (_cleanerLocationsById.TryGetValue(request.AccountId, out var cleanerLocation))
            return new ParamRequestResult<GetLocationResponse>(new Success(), new GetLocationResponse()
            {
                Coordinate = cleanerLocation
            });

        return new ParamRequestResult<GetLocationResponse>(new DataEntryNotFound());
    }

    public RequestResult UpdateLocation(LocationUpdateRequest request)
    {
        using var scope = _serviceProvider.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var account = unitOfWork.AccountRepository.GetById(request.AccountId);
        var userProfile = unitOfWork.UserProfileRepository.GetById(account.UserProfileId);
        if (userProfile.UserRole == UserRole.Driver)
            _driverLocationsById[request.AccountId] = request.NewLocation;
        else if (userProfile.UserRole == UserRole.Cleaner)
            _cleanerLocationsById[request.AccountId] = request.NewLocation;

        return new RequestResult(new Success());
    }

    public void Dispose()
    {
        _locationBroadcastTimer?.Dispose();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        InitLocations();
        _locationBroadcastTimer = new Timer(_ => BroadcastLocation(), null, 0, REFRESH_INTERVAL);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private void InitLocations()
    {
        using var scope = _serviceProvider.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var userProfiles = unitOfWork.UserProfileRepository.GetAll();
        foreach (var userProfile in userProfiles)
        {
            if (userProfile.UserRole == UserRole.Driver)
                _driverLocationsById[userProfile.Id] = new Coordinate(10.7670552457392, 106.656326672901);
            else if (userProfile.UserRole == UserRole.Cleaner)
                _cleanerLocationsById[userProfile.Id] = new Coordinate(10.7670552457392, 106.656326672901);
        }
    }

    private void BroadcastLocation()
    {
        using var scope = _serviceProvider.CreateScope();
        var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<BaseHub>>();

        var broadcastData = new WorkerLocationBroadcastData()
        {
            DriverLocationByIds = _driverLocationsById,
            CleanerLocationByIds = _cleanerLocationsById,
        };

        hubContext.Clients.All.SendAsync(HubHandlers.WorkerLocation.BROADCAST_LOCATION, broadcastData);
    }
}