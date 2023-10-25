using Commons.Models;
using Repositories.Generics;
using Repositories.Managers;

namespace Repositories.Implementations.Tasks;

public class TaskRepository : GenericRepository<TaskData>, ITaskRepository
{
    public TaskRepository(UwcDbContext context) : base(context)
    {
    }
}