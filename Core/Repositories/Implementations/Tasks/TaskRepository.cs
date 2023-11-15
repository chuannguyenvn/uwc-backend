using Commons.Models;
using Repositories.Generics;
using Repositories.Managers;

namespace Repositories.Implementations.Tasks;

public class TaskRepository : GenericRepository<TaskData>, ITaskRepository
{
    public TaskRepository(UwcDbContext context) : base(context)
    {
    }

    public List<TaskData> GetTasksByDate(DateTime date)
    {
        return Context.TaskDataTable.Where(task => task.AssignedTimestamp == date.Date).ToList();
    }

    public List<TaskData> GetTasksByWorkerId(int workerId)
    {
        return Context.TaskDataTable.Where(task => task.AssigneeAccountId == workerId).ToList();
    }
}