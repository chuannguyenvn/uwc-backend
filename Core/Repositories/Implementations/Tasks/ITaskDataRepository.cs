using Commons.Models;
using Repositories.Generics;

namespace Repositories.Implementations.Tasks;

public interface ITaskDataRepository : IGenericRepository<TaskData>
{
    public List<TaskData> GetTasksByDate(DateTime date);
    public List<TaskData> GetTasksByWorkerId(int workerId);
    public List<TaskData> GetWorkerRemainingTasksIn24Hours(int workerId);
    public List<TaskData> GetUnassignedTasksIn24Hours();
    public List<TaskData> GetTasksFromTodayOrFuture();
    public TaskData? GetFocusedTaskByWorkerId(int workerId);
    public void RemoveAllTasksOfWorker(int workerId);
}