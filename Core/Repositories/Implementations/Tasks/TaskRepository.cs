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
        return Context.TaskDatas.Where(task => task.AssignedTimestamp == date.Date).ToList();
    }
}