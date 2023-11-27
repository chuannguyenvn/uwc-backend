using Commons.Extensions;
using Commons.Types;
using MockApplication.Base;

namespace MockApplication.WorkerActivity;

public class WorkerActivityMock : BaseMock
{
    private readonly Dictionary<int, Direction> _ongoingDirectionByDriverAccountIds = new();
    private readonly Dictionary<int, Direction> _ongoingDirectionByCleanerAccountIds = new();

    protected override async Task Main()
    {
        await PickRandomDrivers(1);
        // await PickRandomCleaners(10);
        await MockBehavior();
    }

    private async Task PickRandomDrivers(int countToPick)
    {
        var allDrivers = (await GetAllDriverProfiles()).DriverProfiles;
        var randomDrivers = allDrivers.GetRandom(countToPick);

        foreach (var randomDriver in randomDrivers)
        {
            var newDirection = new Direction(true);
            newDirection.CurrentCoordinate = new Coordinate(10.7670552457392, 106.656326672901);
            _ongoingDirectionByDriverAccountIds[randomDriver.AccountId] = newDirection;
        }
    }

    private async Task PickRandomCleaners(int countToPick)
    {
        var allCleaners = (await GetAllCleanerProfiles()).CleanerProfiles;
        var randomCleaners = allCleaners.GetRandom(countToPick);

        foreach (var randomCleaner in randomCleaners)
        {
            var newDirection = new Direction(true);
            newDirection.CurrentCoordinate = new Coordinate(10.7670552457392, 106.656326672901);
            _ongoingDirectionByCleanerAccountIds[randomCleaner.AccountId] = newDirection;
        }
    }

    private async Task MockBehavior()
    {
        while (true)
        {
            await Task.Delay(1000);

            await MockDriverBehavior();
            await MockCleanerBehavior();
        }
    }

    private async Task MockDriverBehavior()
    {
        foreach (var (id, direction) in _ongoingDirectionByDriverAccountIds)
        {
            if (direction.IsCompleted)
            {
                var randomMcps = (await GetAllMcpData()).Results.GetRandom();
                var newDirection = await GetDirection(id, direction.CurrentCoordinate, randomMcps.Select(mcp => mcp.Id).ToList());

                if (newDirection == null)
                {
                    Console.WriteLine("Failed to get direction");
                    continue;
                }

                _ongoingDirectionByDriverAccountIds[id] = newDirection.Direction;

                Console.WriteLine("New direction for driver {0} is {1}", id, newDirection.Direction);
            }
            else
            {
                var newCoordinate = _ongoingDirectionByDriverAccountIds[id].TravelBy(0.0001);
                Console.WriteLine("Driver {0} is at {1}", id, newCoordinate);
                UpdateLocation(id, newCoordinate);
            }
        }
    }

    private async Task MockCleanerBehavior()
    {
        foreach (var (id, direction) in _ongoingDirectionByCleanerAccountIds)
        {
            if (direction.IsCompleted)
            {
                var randomMcps = (await GetAllMcpData()).Results.GetRandom();
                var newDirection = await GetDirection(id, direction.CurrentCoordinate, randomMcps.Select(mcp => mcp.Id).ToList());

                if (newDirection == null)
                {
                    Console.WriteLine("Failed to get direction");
                    continue;
                }

                _ongoingDirectionByCleanerAccountIds[id] = newDirection.Direction;
            }
            else
            {
                UpdateLocation(id, _ongoingDirectionByCleanerAccountIds[id].TravelBy(0.0001));
            }
        }
    }
}