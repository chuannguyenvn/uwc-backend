using Commons.Communications.Map;
using Commons.Communications.Tasks;
using Commons.Models;
using Commons.Types;
using Repositories.Managers;
using Services.Mcps;
using Services.Tasks;

namespace Services.Map;

public class RouteOptimizationService : IRouteOptimizationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITaskService _taskService;
    private readonly ILocationService _locationService;
    private readonly IMcpFillLevelService _mcpFillLevelService;

    public RouteOptimizationService(IUnitOfWork unitOfWork, ITaskService taskService, ILocationService locationService,
        IMcpFillLevelService mcpFillLevelService)
    {
        _unitOfWork = unitOfWork;
        _taskService = taskService;
        _locationService = locationService;
        _mcpFillLevelService = mcpFillLevelService;
    }

    private List<TaskData> GetWorkerTasksIn24Hours(int workerId)
    {
        return _unitOfWork.TaskDataDataRepository.GetWorkerRemainingTasksIn24Hours(workerId);
    }

    private List<TaskData> GetUnassignedTaskIn24Hours()
    {
        return _unitOfWork.TaskDataDataRepository.GetUnassignedTasksIn24Hours();
    }

    private Dictionary<int, float> GetAllMcpFillLevels()
    {
        return _mcpFillLevelService.GetAllFillLevel().Data.FillLevelsById;
    }

    private Coordinate GetWorkerLocation(int workerId)
    {
        return _locationService.GetLocation(new GetLocationRequest
        {
            AccountId = workerId
        }).Data!.Coordinate;
    }

    private Dictionary<int, UserProfile> GetAllWorkerProfiles()
    {
        return _unitOfWork.UserProfileRepository.GetAllWorkers().ToDictionary(workerProfile => workerProfile.Id);
    }

    private void AssignWorkerToTask(int taskDataId, int workerId)
    {
        _taskService.AssignWorkerToTask(new AssignWorkerToTaskRequest
        {
            TaskId = taskDataId,
            WorkerId = workerId
        });
    }

    public List<TaskData> OptimizeRouteForWorker(UserProfile workerProfile)
    {
        List<TaskData> assignedTasks = GetWorkerTasksIn24Hours(workerProfile.Id);
        List<TaskData> unassignedTasks = GetUnassignedTaskIn24Hours();

        Dictionary<int, float> mcpFillLevels = GetAllMcpFillLevels();
        Coordinate workerLocation = GetWorkerLocation(workerProfile.Id);

        List<TaskData> optimizedTasks = new List<TaskData>();

        // TODO: Optimize route by reorder tasks in assignedTasks

        // Example: Sort tasks by fill level of mcp and deadline
        optimizedTasks = assignedTasks.OrderByDescending(task => mcpFillLevels[task.McpDataId]).ThenBy(task => task.CompleteByTimestamp).ToList();

        return optimizedTasks;
    }

    public void DistributeTasksFromPool()
    {
        List<TaskData> unassignedTasks = GetUnassignedTaskIn24Hours();
        Dictionary<int, float> mcpFillLevels = GetAllMcpFillLevels();
        Dictionary<int, UserProfile> workerProfiles = GetAllWorkerProfiles();

        // TODO: Distribute tasks from unassignedTasks to workers

        // Example: Assign a task to the first worker if the worker is free
        if (GetWorkerTasksIn24Hours(workerProfiles[0].Id).Count == 0)
            AssignWorkerToTask(workerProfiles[0].Id, unassignedTasks[0].Id);
    }
}