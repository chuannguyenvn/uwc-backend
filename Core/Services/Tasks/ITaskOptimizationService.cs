using Commons.Models;

namespace Services.Tasks;

public interface ITaskOptimizationService
{
    public List<TaskData> OptimizeRouteForWorker(UserProfile workerProfile);
    public void DistributeTasksFromPool();
}