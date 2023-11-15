using Commons.Models;
using Repositories.Generics;

namespace Repositories.Implementations.Tasks;

public interface ITaskRepository : IGenericRepository<TaskData>
{
    public List<TaskData> GetTasksByDate(DateTime date);
    public List<TaskData> GetTasksByWorkerId(int workerId);
}