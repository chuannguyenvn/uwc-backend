using Commons.Models;
using Repositories.Generics;
using Repositories.Managers;
using TaskStatus = Commons.Types.TaskStatus;

namespace Repositories.Implementations.Tasks;

public class MockTaskRepository : MockGenericRepository<TaskData>, ITaskRepository
{
    public MockTaskRepository(MockUwcDbContext mockUwcDbContext) : base(mockUwcDbContext)
    {
    }

    public List<TaskData> GetTasksByDate(DateTime date)
    {
        return Context.TaskDataTable.Where(task => task.CompleteByTimestamp == date.Date).ToList();
    }

    public List<TaskData> GetTasksByWorkerId(int workerId)
    {
        return Context.TaskDataTable.Where(task => task.AssigneeId == workerId).ToList();
    }

    public List<TaskData> GetWorkerRemainingTasksIn24Hours(int workerId)
    {
        return Context.TaskDataTable.Where(task =>
            task.AssigneeId == workerId && DateTime.Now.AddHours(24) >= task.CompleteByTimestamp && task.TaskStatus != TaskStatus.Completed).ToList();
    }

    public List<TaskData> GetTasksFromTodayOrFuture()
    {
        return Context.TaskDataTable.Where(task => task.CompleteByTimestamp >= DateTime.Now.Date).ToList();
    }
}