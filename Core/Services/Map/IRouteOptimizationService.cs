using Commons.Models;

namespace Services.Map;

public interface IRouteOptimizationService
{
    public List<TaskData> OptimizeRouteForWorker(UserProfile workerProfile);
}