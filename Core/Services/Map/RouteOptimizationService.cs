using Commons.Communications.Map;
using Commons.Models;
using Commons.Types;
using Repositories.Managers;
using Services.Mcps;

namespace Services.Map;

public class RouteOptimizationService : IRouteOptimizationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocationService _locationService;
    private readonly IMcpFillLevelService _mcpFillLevelService;

    public RouteOptimizationService(IUnitOfWork unitOfWork, ILocationService locationService, IMcpFillLevelService mcpFillLevelService)
    {
        _unitOfWork = unitOfWork;
        _locationService = locationService;
        _mcpFillLevelService = mcpFillLevelService;
    }

    private List<TaskData> GetWorkerTasksIn24Hours(UserProfile workerProfile)
    {
        return _unitOfWork.TaskDataDataRepository.GetWorkerRemainingTasksIn24Hours(workerProfile.Id);
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

    public List<TaskData> OptimizeRoute(UserProfile workerProfile)
    {
        var toDoTasks = GetWorkerTasksIn24Hours(workerProfile);
        if (toDoTasks.Count == 0) return new List<TaskData>();

        var optimizedTasks = new List<TaskData>();
        var mcpFillLevels = GetAllMcpFillLevels();
        var workerLocation = GetWorkerLocation(workerProfile);

        // TODO...
        optimizedTasks = toDoTasks.OrderByDescending(task => mcpFillLevels[task.McpDataId]).ThenBy(task => task.CompleteByTimestamp).ToList();

        return optimizedTasks;
    }
}