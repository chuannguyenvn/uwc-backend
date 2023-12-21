using Commons.Communications.Tasks;

namespace Services.Tasks;

public interface ITaskService
{
    public ParamRequestResult<GetTasksOfWorkerResponse> GetTasksOfWorker(GetTasksOfWorkerRequest request);
    public ParamRequestResult<GetTasksWithMcpResponse> GetTasksWithMcp(GetTasksWithMcpRequest request);
    public ParamRequestResult<GetWorkerPrioritizedTaskResponse> GetWorkerPrioritizedTask(GetWorkerPrioritizedTaskRequest request);
    public ParamRequestResult<GetAllTasksResponse> GetAllTasks();
    public RequestResult AddTask(AddTasksRequest request);
    public RequestResult FocusTask(FocusTaskRequest request);
    public RequestResult CompleteTask(CompleteTaskRequest request);
    public RequestResult RejectTask(RejectTaskRequest request);
}