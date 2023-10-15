using Commons.Communications.Tasks;

namespace Services.Tasks;

public interface ITaskService
{
    public RequestResult AddTask(AddTaskRequest request);
    public RequestResult CompleteTask(CompleteTaskRequest request);
}