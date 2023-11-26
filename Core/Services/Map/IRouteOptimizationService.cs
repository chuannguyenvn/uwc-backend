using Commons.Models;

namespace Services.Map;

public interface IRouteOptimizationService
{
    public List<TaskData> OptimizeRoute(UserProfile workerProfile);
}