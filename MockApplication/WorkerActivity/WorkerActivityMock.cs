using Commons.Extensions;
using Commons.Types;
using MockApplication.Base;

namespace MockApplication.WorkerActivity;

public class WorkerActivityMock : BaseMock
{
    private readonly Dictionary<int, Direction> _ongoingDirectionByDriverAccountIds = new();
    private readonly Dictionary<int, int> _ongoingTaskIdByDriverAccountIds = new();
    private readonly Dictionary<int, Direction> _ongoingDirectionByCleanerAccountIds = new();

    protected override async Task Main()
    {
        await PickRandomDrivers(10);
        // await PickRandomCleaners(10);
        await MockBehavior();
    }

    private async Task PickRandomDrivers(int countToPick)
    {
        // var allDrivers = (await GetAllDriverProfiles()).DriverProfiles.Where(profile => profile.Id != 11).ToList();
        // var randomDrivers = allDrivers.GetRandom(countToPick);
        //
        // foreach (var randomDriver in randomDrivers)
        // {
        //     var newDirection = new Direction();
        //     newDirection.CurrentCoordinate = new Coordinate(10.7670552457392, 106.656326672901);
        //     _ongoingDirectionByDriverAccountIds[randomDriver.AccountId] = newDirection;
        //     _ongoingTaskIdByDriverAccountIds[randomDriver.AccountId] = -1;
        // }

        var direction = new Direction();
        direction.CurrentCoordinate = new Coordinate(10.7670552457392, 106.656326672901);
        _ongoingDirectionByDriverAccountIds[11] = direction;
        _ongoingTaskIdByDriverAccountIds[11] = -1;
    }

    private async Task PickRandomCleaners(int countToPick)
    {
        var allCleaners = (await GetAllCleanerProfiles()).CleanerProfiles;
        var randomCleaners = allCleaners.GetRandom(countToPick);

        foreach (var randomCleaner in randomCleaners)
        {
            var newDirection = new Direction();
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
            // await MockCleanerBehavior();
        }
    }

    private async Task MockDriverBehavior()
    {
        foreach (var (id, direction) in _ongoingDirectionByDriverAccountIds)
        {
            if (direction.IsCompleted)
            {
                if (_ongoingTaskIdByDriverAccountIds[id] != -1) await CompleteTask(id, _ongoingTaskIdByDriverAccountIds[id]);

                var task = GetWorkerPrioritizedTask(id).Result.Task;
                if (task == null)
                {
                    Console.WriteLine("No more tasks for driver {0}", id);
                    continue;
                }

                _ongoingTaskIdByDriverAccountIds[id] = task.Id;

                await FocusTask(id, task.Id);

                var newDirection = await GetDirection(id, direction.CurrentCoordinate, new List<int> { task.McpData.Id });

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
                var newCoordinate = _ongoingDirectionByDriverAccountIds[id].TravelBy(0.00005);
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
                var task = GetWorkerPrioritizedTask(id).Result.Task;
                if (task == null)
                {
                    Console.WriteLine("No more tasks for cleaner {0}", id);
                    continue;
                }

                var newDirection = await GetDirection(id, direction.CurrentCoordinate, new List<int> { task.McpData.Id });

                if (newDirection == null)
                {
                    Console.WriteLine("Failed to get direction");
                    continue;
                }

                _ongoingDirectionByCleanerAccountIds[id] = newDirection.Direction;
            }
            else
            {
                UpdateLocation(id, _ongoingDirectionByCleanerAccountIds[id].TravelBy(0.00005));
            }
        }
    }
}