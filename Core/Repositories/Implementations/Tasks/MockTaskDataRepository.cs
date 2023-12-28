using Commons.Models;
using Repositories.Generics;
using Repositories.Managers;
using TaskStatus = Commons.Types.TaskStatus;

namespace Repositories.Implementations.Tasks;

public class MockTaskDataRepository : MockGenericRepository<TaskData>, ITaskDataRepository
{
    public MockTaskDataRepository(MockUwcDbContext mockUwcDbContext) : base(mockUwcDbContext)
    {
    }

    public override void Add(TaskData entity)
    {
        base.Add(entity);

        if (entity.AssigneeId.HasValue && entity.AssigneeId != 0 && entity.AssigneeProfile == null)
        {
            entity.AssigneeProfile = Context.UserProfileTable.First(profile => profile.Id == entity.AssigneeId);
        }
        else if (entity.AssigneeProfile != null && entity.AssigneeId == 0)
        {
            entity.AssigneeId = entity.AssigneeProfile.Id;
        }

        if (entity.AssignerId != 0)
        {
            entity.AssignerProfile = Context.UserProfileTable.First(profile => profile.Id == entity.AssignerId);
        }
        else if (entity.AssignerProfile != null)
        {
            entity.AssignerId = entity.AssignerProfile.Id;
        }

        if (entity.McpDataId != 0)
        {
            entity.McpData = Context.McpDataTable.First(mcpData => mcpData.Id == entity.McpDataId);
        }
        else if (entity.McpData != null)
        {
            entity.McpDataId = entity.McpData.Id;
        }
        else
        {
            throw new Exception("McpDataId and McpData cannot both be null");
        }
    }

    public List<TaskData> GetTasksByDate(DateTime date)
    {
        return Context.TaskDataTable.Where(task => task.CompleteByTimestamp.Date == date.Date).ToList();
    }

    public List<TaskData> GetTasksByWorkerId(int workerId)
    {
        return Context.TaskDataTable.Where(task => task.AssigneeId == workerId).ToList();
    }

    public List<TaskData> GetTasksByMcpId(int mcpId)
    {
        return Context.TaskDataTable.Where(task => task.McpDataId == mcpId).ToList();
    }

    public List<TaskData> GetWorkerRemainingTasksIn24Hours(int workerId)
    {
        return Context.TaskDataTable.Where(task =>
                task.AssigneeId == workerId && DateTime.Now.AddHours(24) >= task.CompleteByTimestamp && task.TaskStatus == TaskStatus.NotStarted)
            .ToList();
    }

    public List<TaskData> GetUnassignedTasksIn24Hours()
    {
        return Context.TaskDataTable.Where(task =>
            task.AssigneeId == null && DateTime.Now.AddHours(24) >= task.CompleteByTimestamp && task.TaskStatus == TaskStatus.NotStarted).ToList();
    }

    public List<TaskData> GetTasksFromTodayOrFuture()
    {
        return Context.TaskDataTable.Where(task => task.CompleteByTimestamp >= DateTime.Now.Date).ToList();
    }

    public TaskData? GetFocusedTaskByWorkerId(int workerId)
    {
        if (!Context.TaskDataTable.Any(task => task.AssigneeId == workerId && task.TaskStatus == TaskStatus.InProgress)) return null;
        return Context.TaskDataTable.First(task => task.AssigneeId == workerId && task.TaskStatus == TaskStatus.InProgress);
    }

    public TaskData? GetPrioritizedTaskByWorkerId(int workerId)
    {
        if (!Context.TaskDataTable.Any(task => task.AssigneeId == workerId && task.TaskStatus == TaskStatus.NotStarted)) return null;
        return Context.TaskDataTable
            .Where(task => task.AssigneeId == workerId && task.TaskStatus == TaskStatus.NotStarted)
            .MaxBy(task => task.Priority);
    }

    public void RemoveAllTasksOfWorker(int workerId)
    {
        Context.TaskDataTable.RemoveAll(task => task.AssigneeId == workerId);
    }

    public int GetMaxTaskGroupId()
    {
        return Context.TaskDataTable.Max(task => task.GroupId ?? 0);
    }
}