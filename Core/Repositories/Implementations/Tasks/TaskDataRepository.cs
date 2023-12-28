using Commons.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Generics;
using Repositories.Managers;
using TaskStatus = Commons.Types.TaskStatus;

namespace Repositories.Implementations.Tasks;

public class TaskDataRepository : GenericRepository<TaskData>, ITaskDataRepository
{
    public TaskDataRepository(UwcDbContext context) : base(context)
    {
    }

    public List<TaskData> GetTasksByDate(DateTime date)
    {
        return Context.TaskDataTable.Where(task => task.CompleteByTimestamp.Date == date.Date).Include(task => task.McpData).ToList();
    }

    public List<TaskData> GetTasksByWorkerId(int workerId)
    {
        return Context.TaskDataTable.Where(task => task.AssigneeId == workerId)
            .Include(task => task.McpData)
            .Include(task => task.AssigneeProfile)
            .Include(task => task.AssignerProfile).ToList();
    }

    public List<TaskData> GetTasksByMcpId(int mcpId)
    {
        return Context.TaskDataTable.Where(task => task.McpDataId == mcpId)
            .Include(task => task.McpData)
            .Include(task => task.AssigneeProfile)
            .Include(task => task.AssignerProfile).ToList();
    }

    public List<TaskData> GetWorkerRemainingTasksIn24Hours(int workerId)
    {
        return Context.TaskDataTable.Where(task =>
                task.AssigneeId == workerId && DateTime.Now.AddHours(24) >= task.CompleteByTimestamp && task.TaskStatus == TaskStatus.NotStarted)
            .Include(task => task.McpData)
            .Include(task => task.AssigneeProfile)
            .Include(task => task.AssignerProfile).ToList();
    }

    public List<TaskData> GetUnassignedTasksIn24Hours()
    {
        return Context.TaskDataTable.Where(task =>
                task.AssigneeId == null && DateTime.Now.AddHours(24) >= task.CompleteByTimestamp && task.TaskStatus == TaskStatus.NotStarted)
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

    public TaskData? GetFocusedTaskByWorkerId(int workerId)
    {
        if (!Context.TaskDataTable.Any(task => task.AssigneeId == workerId && task.TaskStatus == TaskStatus.InProgress)) return null;
        return Context.TaskDataTable
            .Include(task => task.McpData)
            .Include(task => task.AssigneeProfile)
            .Include(task => task.AssignerProfile)
            .First(task => task.AssigneeId == workerId && task.TaskStatus == TaskStatus.InProgress);
    }

    public TaskData? GetPrioritizedTaskByWorkerId(int workerId)
    {
        if (!Context.TaskDataTable.Any(task => task.AssigneeId == workerId && task.TaskStatus == TaskStatus.NotStarted)) return null;
        return GetWorkerRemainingTasksIn24Hours(workerId).MaxBy(task => task.Priority);
    }

    public void RemoveAllTasksOfWorker(int workerId)
    {
        var tasks = Context.TaskDataTable.Where(task => task.AssigneeId == workerId).ToList();
        Context.TaskDataTable.RemoveRange(tasks);
    }

    public int GetMaxTaskGroupId()
    {
        return Context.TaskDataTable.Max(task => task.GroupId ?? 0);
    }
}