using Commons.Models;
using Repositories.Generics;

namespace Repositories.Implementations.Tasks;

public interface ITaskDataRepository : IGenericRepository<TaskData>
{
    public List<TaskData> GetTasksByDate(DateTime date);
    public List<TaskData> GetTasksByWorkerId(int workerId);
    public List<TaskData> GetTasksByMcpId(int mcpId);
    public List<TaskData> GetWorkerRemainingTasksIn24Hours(int workerId);
    public List<TaskData> GetWorkerFiveUpcomingTasks(int workerId);
    public List<TaskData> GetUnassignedTasksIn24Hours();
    public List<TaskData> GetTasksFromTodayOrFuture();
    public TaskData? GetFocusedTaskByWorkerId(int workerId);
    public TaskData? GetPrioritizedTaskByWorkerId(int workerId);
    public TaskData? GetNextMcpTask(int mcpId);
    public void RemoveAllTasksOfWorker(int workerId);
    public int GetMaxTaskGroupId();
}