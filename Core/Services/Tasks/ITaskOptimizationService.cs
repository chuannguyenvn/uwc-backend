using Commons.Communications.Tasks;
using Commons.Models;

namespace Services.Tasks;

public interface ITaskOptimizationService
{
    public List<TaskData> OptimizeRouteForWorker(UserProfile workerProfile);
    public void DistributeTasksFromPool();
    public void ProcessAddTaskRequest(AddTasksRequest request);
    public void ToggleAutoTaskDistribution(bool isOn);
}