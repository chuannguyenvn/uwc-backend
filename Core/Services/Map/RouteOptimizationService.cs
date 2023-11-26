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

    private List<TaskData> GetWorkerTasksIn24Hours(UserProfile workerProfile)
    {
        return _unitOfWork.TaskDataDataRepository.GetWorkerRemainingTasksIn24Hours(workerProfile.Id);
    }

    private List<TaskData> GetUnassignedTaskIn24Hours()
    {
        return _unitOfWork.TaskDataDataRepository.GetUnassignedTasksIn24Hours();
    }

    private Dictionary<int, float> GetAllMcpFillLevels()
    {
        return _mcpFillLevelService.GetAllFillLevel().Data.FillLevelsById;
    }

    private Coordinate GetWorkerLocation(UserProfile workerProfile)
    {
        return _locationService.GetLocation(new GetLocationRequest
        {
            AccountId = workerProfile.AccountId
        }).Data!.Coordinate;
    }

    private void AssignWorkerToTask(TaskData taskData, UserProfile workerProfile)
    {
        _taskService.AssignWorkerToTask(new AssignWorkerToTaskRequest
        {
            TaskId = taskData.Id,
            WorkerId = workerProfile.Id
        });
    }

    public List<TaskData> OptimizeRoute(UserProfile workerProfile)
    {
        var assignedTasks = GetWorkerTasksIn24Hours(workerProfile);
        var unassignedTasks = GetUnassignedTaskIn24Hours();

        var mcpFillLevels = GetAllMcpFillLevels();
        var workerLocation = GetWorkerLocation(workerProfile);

        // TODO...
        var optimizedTasks = new List<TaskData>();
        optimizedTasks = assignedTasks.OrderByDescending(task => mcpFillLevels[task.McpDataId]).ThenBy(task => task.CompleteByTimestamp).ToList();

        return optimizedTasks;
    }
}