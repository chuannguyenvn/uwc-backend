using Commons.Models;
using Repositories.Managers;
using Services.Mcps;

namespace Services.Map;

public class RouteOptimizationService : IRouteOptimizationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMcpFillLevelService _mcpFillLevelService;

    public RouteOptimizationService(IUnitOfWork unitOfWork, IMcpFillLevelService mcpFillLevelService)
    {
        _unitOfWork = unitOfWork;
        _mcpFillLevelService = mcpFillLevelService;
    }

    private List<TaskData> GetWorkerTasksIn24Hours(UserProfile workerProfile)
    {
        return _unitOfWork.TaskDataRepository.GetWorkerRemainingTasksIn24Hours(workerProfile.Id);
    }

    private Dictionary<int, float> GetMcpFillLevels()
    {
        return _mcpFillLevelService.GetAllFillLevel().Data.FillLevelsById;
    }

    public List<TaskData> OptimizeRoute(UserProfile workerProfile)
    {
        var toDoTasks = GetWorkerTasksIn24Hours(workerProfile);
        if (toDoTasks.Count == 0) return new List<TaskData>();

        var optimizedTasks = new List<TaskData>();
        var mcpFillLevels = GetMcpFillLevels();

        // TODO...

        return optimizedTasks;
    }
}