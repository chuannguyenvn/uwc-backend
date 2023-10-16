﻿using Commons.Categories;
using Commons.Communications.Map;
using Commons.Types;
using Repositories.Managers;
using Services.Mcps;

namespace Services.Map;

public class MockDrivingBehaviorService : IHostedService
{
    private readonly Dictionary<int, MockCurrentRoute> _ongoingRouteByDriverAccountIds = new();

    private readonly IServiceProvider _serviceProvider;

    public MockDrivingBehaviorService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        PickRandomDrivers(1);
        RandomizeDrivingBehavior();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private void PickRandomDrivers(int countToPick)
    {
        using var scope = _serviceProvider.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var randomDrivers = unitOfWork.Accounts.GetRandomWithCondition(account => account.UserRole == UserRole.Driver, countToPick);

        foreach (var randomDriver in randomDrivers)
        {
            _ongoingRouteByDriverAccountIds[randomDriver.Id] = new MockCurrentRoute();
        }
    }

    private async void RandomizeDrivingBehavior()
    {
        using var scope = _serviceProvider.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var locationService = scope.ServiceProvider.GetRequiredService<ILocationService>();
        var directionService = scope.ServiceProvider.GetRequiredService<IDirectionService>();
        var mcpFillLevelService = scope.ServiceProvider.GetRequiredService<IMcpFillLevelService>();

        while (true)
        {
            bool first = true;
            foreach (var (id, direction) in _ongoingRouteByDriverAccountIds.ToList())
            {
                if (direction.IsCompleted)
                {
                    var randomMcps = unitOfWork.McpData.GetRandom(5).ToList();
                    var waypoints = randomMcps.Select(mcp => mcp.Coordinate).ToList();
                    var newDirection = directionService.GetDirection(new GetDirectionRequest()
                    {
                        AccountId = id,
                        CurrentLocation = locationService.LocationsByAccountId[id],
                        Destinations = waypoints,
                    }).Data?.Direction;

                    if (newDirection == null)
                    {
                        Console.WriteLine("Failed to get direction");
                        continue;
                    }

                    _ongoingRouteByDriverAccountIds[id] = new MockCurrentRoute(
                        locationService.LocationsByAccountId[id],
                        randomMcps.Select(mcp => mcp.Id).ToList(),
                        newDirection,
                        mcpId => mcpFillLevelService.EmptyMcp(mcpId));
                }
                else
                {
                    locationService.LocationsByAccountId[id] = _ongoingRouteByDriverAccountIds[id].TravelBy(0.0001);
                }
                
                if (first)
                {
                    Console.WriteLine($"Driver {id} is at {_ongoingRouteByDriverAccountIds[id].CurrentCoordinate}");
                    first = false;
                }
            }

            await Task.Delay(1000);
        }
    }
}