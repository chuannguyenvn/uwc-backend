using Commons.Models;
using Repositories.Generics;
using Repositories.Managers;

namespace Repositories.Implementations.Tasks;

public class MockTaskRepository : MockGenericRepository<TaskData>, ITaskRepository
{
    public MockTaskRepository(MockUwcDbContext mockUwcDbContext) : base(mockUwcDbContext)
    {
    }

    public List<TaskData> GetTasksByDate(DateTime date)
    {
        return Context.TaskDataTable.Where(task => task.AssignedTimestamp.Date == date.Date).ToList();
    }
}