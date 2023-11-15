using Commons.Communications.Tasks;

namespace Services.Tasks;

public interface ITaskService
{
    public ParamRequestResult<GetTasksOfWorkerResponse> GetTasksOfWorker(GetTasksOfWorkerRequest request);
    public ParamRequestResult<GetAllTasksResponse> GetAllTasks();
    public RequestResult AddTask(AddTaskRequest request);
    public RequestResult CompleteTask(CompleteTaskRequest request);
    public RequestResult RejectTask(RejectTaskRequest request);
}