using Commons.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Generics;
using Repositories.Managers;
using TaskStatus = Commons.Types.TaskStatus;

namespace Repositories.Implementations.Tasks;

public class TaskDataDataRepository : GenericRepository<TaskData>, ITaskDataRepository
{
    public TaskDataDataRepository(UwcDbContext context) : base(context)
    {
    }

    public List<TaskData> GetTasksByDate(DateTime date)
    {
        return Context.TaskDataTable.Where(task => task.CompleteByTimestamp == date.Date).Include(task => task.McpData).ToList();
    }

    public List<TaskData> GetTasksByWorkerId(int workerId)
    {
        return Context.TaskDataTable.Where(task => task.AssigneeId == workerId)
            .Include(task => task.McpData)
            .Include(task => task.AssigneeProfile)
            .Include(task => task.AssignerProfile).ToList();
    }

    public List<TaskData> GetWorkerRemainingTasksIn24Hours(int workerId)
    {
        return Context.TaskDataTable.Where(task =>
                task.AssigneeId == workerId && DateTime.Now.AddHours(24) >= task.CompleteByTimestamp && task.TaskStatus != TaskStatus.Completed)
            .Include(task => task.McpData)
            .Include(task => task.AssigneeProfile)
            .Include(task => task.AssignerProfile).ToList();
    }

    public List<TaskData> GetUnassignedTasksIn24Hours()
    {
        return Context.TaskDataTable.Where(task =>
                task.AssigneeId == null && DateTime.Now.AddHours(24) >= task.CompleteByTimestamp && task.TaskStatus != TaskStatus.Completed)
            .Include(task => task.McpData)
            .Include(task => task.AssigneeProfile)
            .Include(task => task.AssignerProfile).ToList();
    }

    public List<TaskData> GetTasksFromTodayOrFuture()
    {
        return Context.TaskDataTable.Where(task => task.CompleteByTimestamp >= DateTime.Now.Date)
            .Include(task => task.McpData)
            .Include(task => task.AssigneeProfile)
            .Include(task => task.AssignerProfile).ToList();
    }
}