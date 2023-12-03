using Commons.Categories;
using Commons.Communications.Map;
using Commons.Communications.Mcps;
using Commons.Communications.Tasks;
using Commons.Models;
using Commons.Types;
using Repositories.Managers;
using Services.Map;
using Services.Mcps;
using Services.OnlineStatus;

namespace Services.Tasks;

public class TaskOptimizationService : ITaskOptimizationService
{
    public static Dictionary<int, List<int>> TaskIdsByWorkerId = new();

    private readonly IUnitOfWork _unitOfWork;
    private readonly ITaskService _taskService;
    private readonly ILocationService _locationService;
    private readonly IMcpFillLevelService _mcpFillLevelService;
    private readonly IOnlineStatusService _onlineStatusService;

    private readonly TaskOptimizationServiceHelper _helper;

    private const float FILL_LEVEL_ASSIGNMENT_THRESHOLD = 0.85f;

    private Timer? _autoDistributionTimer;
    private const int AUTO_DISTRIBUTION_INTERVAL_SECONDS = 10;

    public TaskOptimizationService(IUnitOfWork unitOfWork, ITaskService taskService, ILocationService locationService,
        IMcpFillLevelService mcpFillLevelService, IOnlineStatusService onlineStatusService)
    {
        _unitOfWork = unitOfWork;
        _taskService = taskService;
        _locationService = locationService;
        _mcpFillLevelService = mcpFillLevelService;
        _onlineStatusService = onlineStatusService;

        _helper = new TaskOptimizationServiceHelper(unitOfWork, taskService, locationService, mcpFillLevelService, onlineStatusService);
    }


    // --------------------------------------- GEN2 WITH DIJKSTRA ALGORITHM --------------------------------------------
    private double CalculateCost(UserProfile workerProfile, TaskData taskData)
    {
        Coordinate workerLocation = _helper.GetWorkerLocation(workerProfile.Id);
        Coordinate mcpLocation = _helper.GetMcpCoordinateById(taskData.McpDataId);
        double cost = Double.Sqrt(Double.Pow(workerLocation.Latitude - mcpLocation.Latitude, 2) +
                                  Double.Pow(workerLocation.Longitude - mcpLocation.Longitude, 2));
        return cost;
    }

    private double CalculateCost(TaskData taskData1, TaskData taskData2)
    {
        Coordinate mcp1Location = _helper.GetMcpCoordinateById(taskData1.McpDataId);
        Coordinate mcp2Location = _helper.GetMcpCoordinateById(taskData2.McpDataId);
        double cost = Double.Sqrt(Double.Pow(mcp1Location.Latitude - mcp2Location.Latitude, 2) +
                                  Double.Pow(mcp1Location.Longitude - mcp2Location.Longitude, 2));
        return cost;
    }

    // Maybe - Refactor: Code is longer than 60 lines
    private List<double> Dijkstra(UserProfile workerProfile, List<TaskData> assignedTasks, int start = 0)
    {
        int graphSize = assignedTasks.Count + 1;

        // Build a graph based on the taskList
        List<List<(int, double)>> graph = new List<List<(int, double)>>(graphSize);

        for (int i = 0; i < graphSize; i++)
        {
            graph.Add(new List<(int, double)>(graphSize));
        }

        List<int> path = new List<int>(new int[graphSize].Select(_ => -1));
        List<double> dist = new List<double>(new double[graphSize].Select(_ => 1e8));

        var pq = new SortedSet<(int, double)>(Comparer<(int, double)>.Create((a, b) => a.Item2.CompareTo(b.Item2)));
        pq.Add((start, 0.0f));

        for (int index = 0; index < assignedTasks.Count; index++)
        {
            double cost = CalculateCost(workerProfile, assignedTasks[index]);
            graph[0].Add((index + 1, cost));
            graph[index + 1].Add((0, cost));
        }

        for (int i = 0; i < assignedTasks.Count - 1; i++)
        {
            for (int j = i + 1; j < assignedTasks.Count; j++)
            {
                double cost = CalculateCost(assignedTasks[i], assignedTasks[j]);
                graph[i + 1].Add((j + 1, cost));
                graph[j + 1].Add((i + 1, cost));
            }
        }

        // Dijkstra to optimize
        dist[start] = 0;

        while (pq.Count > 0)
        {
            (int loc, double cost) = pq.Min;
            pq.Remove((loc, cost));

            foreach ((int neighbor, double neighborCost) in graph[loc])
            {
                if (cost + neighborCost < dist[neighbor])
                {
                    dist[neighbor] = cost + neighborCost;
                    pq.Add((neighbor, dist[neighbor]));
                    path[neighbor] = loc;
                }
            }
        }

        // Reorder the task list and return the optimized list
        List<(TaskData, double)> combined = new List<(TaskData, double)>();

        for (int i = 0; i < assignedTasks.Count; i++)
        {
            combined.Add((assignedTasks[i], dist[i + 1]));
        }

        combined.Sort((a, b) => a.Item2.CompareTo((b.Item2)));
        List<TaskData> sortedTaskList = combined.Select(pair => pair.Item1).ToList();

        return dist;
    }

    private Tuple<List<TaskData>, double> OptimizeRouteGen2WithDijkstraList(UserProfile workerProfile, ref List<TaskData> assignedTasks)
    {
        List<TaskData> copiedAssignedTasks = assignedTasks;

        // Run the optimization algorithm, passing the assigned Task list as reference
        List<TaskData> result = new List<TaskData>();

        // Create the visited list to avoid edge case and sinkhole (if happens)
        List<bool> visited = new List<bool>(assignedTasks.Count + 1);
        for (int i = 0; i < assignedTasks.Count + 1; i++) visited.Add(false);
        visited[0] = true;

        int index = 0;
        double totalCost = 0;

        for (int i = 0; i < assignedTasks.Count; i++)
        {
            List<TaskData> cloneAssignedTasks = copiedAssignedTasks;
            double cost = 1e9;
            int start = index;

            assignedTasks = cloneAssignedTasks;
            List<double> dist = Dijkstra(workerProfile, assignedTasks, start);

            for (int j = 0; j < dist.Count; j++)
            {
                if (cost > dist[j] && visited[j] is false)
                {
                    cost = dist[j];
                    index = j;
                }
            }

            result.Add(copiedAssignedTasks[index - 1]);
            visited[index] = true;
            totalCost += cost;
        }

        return Tuple.Create(result, totalCost);
    }

    private List<TaskData> OptimizeRouteGen2WithDijkstra(UserProfile workerProfile)
    {
        // Get the worker taskList
        List<TaskData> assignedTasks = _helper.GetWorkerTasksIn24Hours(workerProfile.Id);
        return OptimizeRouteGen2WithDijkstraList(workerProfile, ref assignedTasks).Item1;
    }

    // ----------------------------------- GEN2 WITH PERMUTATION SOLUTION ----------------------------------------------
    private double TotalDistance(UserProfile workerProfile, List<TaskData> dataList)
    {
        double totalDist = CalculateCost(workerProfile, dataList[0]);
        for (int index = 0; index < dataList.Count - 1; index++)
        {
            totalDist += CalculateCost(dataList[index], dataList[index + 1]);
        }

        return totalDist;
    }

    static List<List<TaskData>> GetPermutation(List<TaskData> list)
    {
        var result = new List<List<TaskData>>();

        void Recurse(int index)
        {
            if (index == list.Count - 1)
            {
                result.Add(list.ToList());
                return;
            }

            for (int i = index; i < list.Count; i++)
            {
                var temp = list[index];
                list[index] = list[i];
                list[i] = temp;

                Recurse(index + 1);

                temp = list[index];
                list[index] = list[i];
                list[i] = temp;
            }
        }

        Recurse(0);
        return result;
    }

    private Tuple<List<TaskData>, double> OptimizeRouteGen2WithPermutationList(UserProfile workerProfile, ref List<TaskData> dataList)
    {
        var allPermutations = GetPermutation(dataList);

        double minCost = double.MaxValue;
        List<TaskData> minCostPermutation = new List<TaskData>();

        foreach (var permutation in allPermutations)
        {
            double currentCost = TotalDistance(workerProfile, permutation);
            if (currentCost < minCost)
            {
                minCost = currentCost;
                minCostPermutation = permutation;
            }
        }

        double totalCost = minCost;

        if (allPermutations.Count == 0)
        {
            totalCost = 0;
        }

        return Tuple.Create(minCostPermutation, totalCost);
    }

    private List<TaskData> OptimizeRouteGen2WithPermutation(UserProfile workerProfile)
    {
        List<TaskData> assignedTasks = _helper.GetWorkerTasksIn24Hours(workerProfile.Id);
        return OptimizeRouteGen2WithPermutationList(workerProfile, ref assignedTasks).Item1;
    }

    // ------------------------------------ GEN 2 PRIORITY HANDLING ----------------------------------------------------
    public List<TaskData> OptimizeRouteForWorker(UserProfile workerProfile)
    {
        return new List<TaskData>();
    }

    private List<TaskData> OptimizeRouteForWorker(UserProfile workerProfile, List<bool> priority = null)
    {
        List<TaskData> assignedTasks = _helper.GetWorkerTasksIn24Hours(workerProfile.Id);
        List<TaskData> unassignedTasks = _helper.GetUnassignedTaskIn24Hours();

        Dictionary<int, float> mcpFillLevels = _helper.GetAllMcpFillLevels();
        Coordinate workerLocation = _helper.GetWorkerLocation(workerProfile.Id);

        List<TaskData> optimizedTasks = new List<TaskData>();

        // Example: Sort tasks by fill level of mcp and deadline
        if (priority[0] is true && priority[1] is true)
        {
            optimizedTasks = assignedTasks.OrderBy(task => task.CompleteByTimestamp)
                .ThenBy(task => mcpFillLevels[task.McpDataId]).ToList();
        }
        else
        {
            if (priority[0] is true)
            {
                optimizedTasks = assignedTasks.OrderBy(task => task.CompleteByTimestamp).ToList();
            }
            else
            {
                optimizedTasks = assignedTasks.OrderByDescending(task => mcpFillLevels[task.McpDataId]).ToList();
            }
        }

        return optimizedTasks;
    }

    private async Task<List<TaskData>> OptimizeRouteForWorker(UserProfile workerProfile, List<RoutingOptimizationStrategy>? strategies = null)
    {
        List<TaskData> optimizedTasks = new List<TaskData>();

        if (strategies != null)
        {
            foreach (var strategy in strategies)
            {
                optimizedTasks = await SortTaskList(optimizedTasks, strategy);
            }
        }

        return optimizedTasks;
    }

    // Async as sorting by distance might require multi-threading?
    private async Task<List<TaskData>> SortTaskList(List<TaskData> list, RoutingOptimizationStrategy strategy)
    {
        switch (strategy)
        {
            case RoutingOptimizationStrategy.ByDistance:
                // TODO: ???
                return new List<TaskData>();
            case RoutingOptimizationStrategy.ByDeadline:
                return list.OrderBy(task => task.CompleteByTimestamp).ToList();
            case RoutingOptimizationStrategy.ByFillLevel:
                var mcpFillLevels = _helper.GetAllMcpFillLevels();
                return list.OrderByDescending(task => mcpFillLevels[task.McpDataId]).ToList();
            default:
                throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
        }
    }

    // ------------------------------------------ GEN2 GENERAL ---------------------------------------------------------
    public List<TaskData> OptimizeRouteGen2(UserProfile workerProfile, bool option = true, List<bool> priority = null)
    {
        // Priority[0] => deadline prioritization
        // Priority[1] => fill level prioritization

        // No priority --> use distance as normal
        if (priority is null)
        {
            if (option)
            {
                return OptimizeRouteGen2WithDijkstra(workerProfile);
            }
            else
            {
                return OptimizeRouteGen2WithPermutation(workerProfile);
            }
        }

        if (priority[0] is false && priority[1] is false)
        {
            if (option)
            {
                return OptimizeRouteGen2WithDijkstra(workerProfile);
            }
            else
            {
                return OptimizeRouteGen2WithPermutation(workerProfile);
            }
        }
        else
        {
            return OptimizeRouteForWorker(workerProfile, priority);
        }
    }

    // --------------------------- GEN3: TASK DISTRIBUTION FROM THE COMMON POOL ----------------------------------------
    // Maybe - refactor: Code is longer than 150 lines
    public List<List<TaskData>> DistributeTasksFromPoolGen3(Dictionary<int, UserProfile> workerProfiles, bool costOrFast = true, bool option = true,
        List<bool> priority = null)
    {
        // Cost: Minimize the cost of the additional task
        // Fast: Minimize the maximum travel time of any driver

        List<TaskData> unassignedTasks = _helper.GetUnassignedTaskIn24Hours();
        List<TaskData> sortedUnassignedTasks = new List<TaskData>();

        if (priority is not null && priority[0] == true)
        {
            sortedUnassignedTasks = unassignedTasks.OrderBy(task => task.CompleteByTimestamp).ToList();
        }
        else
        {
            Dictionary<int, float> allMcpFillLevels = _helper.GetAllMcpFillLevels();
            Dictionary<int, float> mcpFillLevels = new Dictionary<int, float>();

            foreach (var task in unassignedTasks)
            {
                int mcpDataId = task.McpDataId;

                // Check if the mcpDataId exists in allMcpFillLevels before adding to mcpFillLevels
                if (allMcpFillLevels.ContainsKey(mcpDataId))
                {
                    float fillLevel = allMcpFillLevels[mcpDataId];

                    // Use the mcpDataId as the key in mcpFillLevels
                    mcpFillLevels[mcpDataId] = fillLevel;
                }
            }

            Dictionary<int, UserProfile> assignedTasks = new Dictionary<int, UserProfile>();

            // Sort the task list based on the priority of chosen by the supervisor
            List<(TaskData, float)> combined = new List<(TaskData, float)>();

            for (int idx = 0; idx < unassignedTasks.Count; idx++)
            {
                combined.Add((unassignedTasks[idx], mcpFillLevels[unassignedTasks[idx].McpDataId]));
            }

            combined.Sort((a, b) => b.Item2.CompareTo((a.Item2)));
            sortedUnassignedTasks = combined.Select(pair => pair.Item1).ToList();
        }

        // Distribute task from the pool to either
        // 1. Minimize the additional cost of the task
        // 2. Minimize the maximal travel time of any worker

        double minMaxCost = 1e9;
        int index = 0;

        for (int i = 0; i < sortedUnassignedTasks.Count; i++)
        {
            foreach (var (workerId, workerProfile) in workerProfiles)
            {
                if (workerProfile.UserRole != UserRole.Driver) continue;
                if (!_helper.IsWorkerOnline(workerProfile.Id)) continue;

                // Switch case based on CostOrFast
                // Cost: Compare delta(cost after - cost before)
                // Fast: Compare (total time after - total time before)

                // Run Dijkstra to optimize: Cost before
                List<TaskData> tempTaskList = _helper.GetWorkerTasksIn24Hours(workerProfile.Id);

                double costBefore = 1e9;
                if (option)
                {
                    Tuple<List<TaskData>, double> optimizedTempTaskList =
                        OptimizeRouteGen2WithDijkstraList(workerProfile, ref tempTaskList);

                    costBefore = optimizedTempTaskList.Item2;
                }
                else
                {
                    Tuple<List<TaskData>, double> optimizedTempTaskList =
                        OptimizeRouteGen2WithPermutationList(workerProfile, ref tempTaskList);

                    costBefore = optimizedTempTaskList.Item2;
                }

                // Run Dijkstra to optimize: Cost after
                tempTaskList = _helper.GetWorkerTasksIn24Hours(workerProfile.Id);


                TaskData trialAndError = new TaskData
                {
                    AssigneeId = workerProfile.Id,
                    AssigneeProfile = workerProfile,
                    McpDataId = sortedUnassignedTasks[i].McpDataId,
                    CreatedTimestamp = DateTime.Now,
                    CompleteByTimestamp = DateTime.Now.AddHours(1)
                };
                tempTaskList.Add(trialAndError);

                double costAfter = 1e9;
                if (option)
                {
                    Tuple<List<TaskData>, double> optimizedTempTaskList =
                        OptimizeRouteGen2WithDijkstraList(workerProfile, ref tempTaskList);

                    costAfter = optimizedTempTaskList.Item2;
                }
                else
                {
                    Tuple<List<TaskData>, double> optimizedTempTaskList =
                        OptimizeRouteGen2WithPermutationList(workerProfile, ref tempTaskList);

                    costAfter = optimizedTempTaskList.Item2;
                }

                if (costOrFast)
                {
                    double deltaCost = costAfter - costBefore;

                    if (deltaCost < minMaxCost)
                    {
                        minMaxCost = deltaCost;
                        index = workerId;
                    }
                }
                else
                {
                    double totalCost = costAfter;
                    if (totalCost < minMaxCost)
                    {
                        minMaxCost = totalCost;
                        index = workerId;
                    }
                }
            }

            if (index != 0)
            {
                foreach (var (workerId, workerProfile) in workerProfiles)
                {
                    if (workerProfile.UserRole != UserRole.Driver)
                    {
                        continue;
                    }

                    if (index == workerId)
                    {
                        _taskService.AssignWorkerToTask(sortedUnassignedTasks[i].Id, index);
                        break;
                    }
                }
            }
        }

        List<List<TaskData>> result = new List<List<TaskData>>();
        foreach (var (workerId, workerProfile) in workerProfiles)
        {
            List<TaskData> optimizedTaskList = OptimizeRouteGen2(workerProfile, option);
            result.Add(optimizedTaskList);
        }

        return result;
    }

    public void ProcessAddTaskRequest(AddTasksRequest request)
    {
        if (request.RoutingOptimizationScope == RoutingOptimizationScope.None)
        {
            // Supervisor specified not to optimize anything
            // => GEN 1

            // Impossible
            if (request.AssigneeAccountId == null) throw new Exception("AssigneeAccountId cannot be null");
            
            foreach (var mcpDataId in request.McpDataIds)
            {
                _taskService.AddTaskWithWorker(request.AssignerAccountId, request.AssigneeAccountId.Value, mcpDataId, request.CompleteByTimestamp);
            }
        }
        else
        {
            if (request.AssigneeAccountId != null)
            {
                // Supervisor specified to optimize something, and provide a worker to assign to
                // => GEN 2
                
                // TODO
            }
            else
            {
                // Supervisor specified to optimize something, but did not provide a worker to assign to
                // => GEN 3
                
                // TODO
            }
        }
    }

    // ------------------------------------ GEN 4: ADD TASKS TO POOL ---------------------------------------------------

    // Supervisor use this to toggle the auto task distribution feature
    public void ToggleAutoTaskDistribution(bool isOn)
    {
        if (isOn)
        {
            _autoDistributionTimer = new Timer(Automation, null, 0, AUTO_DISTRIBUTION_INTERVAL_SECONDS);
        }
        else
        {
            _autoDistributionTimer?.Dispose();
        }
    }

    // This method run one optimization attempt every few seconds
    // Not testable
    private void Automation(object? state = null)
    {
        DistributeTasksFromPool();
    }

    // A single optimization attempt
    // Testable
    public void DistributeTasksFromPool()
    {
        Dictionary<int, float> mcpFillLevels = _helper.GetAllMcpFillLevels();

        foreach (var (mcpId, mcpFillLevel) in mcpFillLevels)
        {
            if (mcpFillLevel >= FILL_LEVEL_ASSIGNMENT_THRESHOLD)
            {
                SetFillLevelRequest mcpSetFillLevel = new SetFillLevelRequest
                {
                    McpId = mcpId,
                    FillLevel = mcpFillLevel - 0.01f
                };
                _mcpFillLevelService.SetFillLevel(mcpSetFillLevel);

                var addTaskRequest = new AddTasksRequest
                {
                    AssignerAccountId = 1,
                    AssigneeAccountId = null,
                    McpDataIds = new List<int>() { mcpId },
                    CompleteByTimestamp = DateTime.Now.AddHours(1),
                };
                _taskService.ProcessAddTaskRequest(addTaskRequest);
            }
        }

        DistributeTasksFromPoolGen3(_helper.GetAllWorkerProfiles(), true, true);
    }
}