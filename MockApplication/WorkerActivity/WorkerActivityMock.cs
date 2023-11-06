using Commons.Extensions;
using Commons.Types;
using MockApplication.Base;

namespace MockApplication.WorkerActivity;

public class WorkerActivityMock : BaseMock
{
    private readonly Dictionary<int, MockCurrentRoute> _ongoingRouteByDriverAccountIds = new();
    private readonly Dictionary<int, MockCurrentRoute> _ongoingRouteByCleanerAccountIds = new();

    protected override async Task Main()
    {
        PickRandomDrivers(10);
        PickRandomCleaners(10);
        await MockBehavior();
    }

    private async void PickRandomDrivers(int countToPick)
    {
        var allDrivers = (await GetAllDriverProfiles()).DriverProfiles;
        var randomDrivers = allDrivers.GetRandom(countToPick);

        foreach (var randomDriver in randomDrivers)
        {
            _ongoingRouteByDriverAccountIds[randomDriver.AccountId] = new MockCurrentRoute();
        }
    }

    private async void PickRandomCleaners(int countToPick)
    {
        var allCleaners = (await GetAllCleanerProfiles()).CleanerProfiles;
        var randomCleaners = allCleaners.GetRandom(countToPick);

        foreach (var randomCleaner in randomCleaners)
        {
            _ongoingRouteByCleanerAccountIds[randomCleaner.AccountId] = new MockCurrentRoute();
        }
    }

    private async Task MockBehavior()
    {
        while (true)
        {
            MockDriverBehavior();
            MockCleanerBehavior();

            await Task.Delay(1000);
        }
    }

    private async void MockDriverBehavior()
    {
        foreach (var (id, direction) in _ongoingRouteByDriverAccountIds.ToList())
        {
            if (direction.IsCompleted)
            {
                var randomMcps = (await GetAllMcpData()).Results.GetRandom(5);
                var waypoints = randomMcps.Select(mcp => mcp.Coordinate).ToList();
                var currentLocation = await GetLocation(id);
                var newDirection = await GetDirection(id, currentLocation, waypoints);

                if (newDirection == null)
                {
                    Console.WriteLine("Failed to get direction");
                    continue;
                }

                _ongoingRouteByDriverAccountIds[id] = new MockCurrentRoute(
                    currentLocation,
                    randomMcps.Select(mcp => mcp.Id).ToList(),
                    newDirection.Direction,
                    mcpId => EmptyMcp(mcpId, id));
            }
            else
            {
                UpdateLocation(id, _ongoingRouteByDriverAccountIds[id].TravelBy(0.0001));
            }
        }
    }

    private async void MockCleanerBehavior()
    {
        foreach (var (id, direction) in _ongoingRouteByCleanerAccountIds.ToList())
        {
            if (direction.IsCompleted)
            {
                var randomMcps = (await GetAllMcpData()).Results.GetRandom(5);
                var waypoints = randomMcps.Select(mcp => mcp.Coordinate).ToList();
                var currentLocation = await GetLocation(id);
                var newDirection = await GetDirection(id, currentLocation, waypoints);

                if (newDirection == null)
                {
                    Console.WriteLine("Failed to get direction");
                    continue;
                }

                _ongoingRouteByCleanerAccountIds[id] = new MockCurrentRoute(
                    currentLocation,
                    randomMcps.Select(mcp => mcp.Id).ToList(),
                    newDirection.Direction,
                    mcpId => EmptyMcp(mcpId, id));
            }
            else
            {
                UpdateLocation(id, _ongoingRouteByCleanerAccountIds[id].TravelBy(0.0001));
            }
        }
    }
}