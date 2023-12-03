using Commons.Communications.Tasks;

namespace Services.Tasks;

public interface ITaskService
{
    public ParamRequestResult<GetTasksOfWorkerResponse> GetTasksOfWorker(GetTasksOfWorkerRequest request);
    public ParamRequestResult<GetAllTasksResponse> GetAllTasks();
    public RequestResult ProcessAddTaskRequest(AddTasksRequest request);
    public void AddTaskWithWorker(int assignerId, int workerId, int mcpId, DateTime completeByTimestamp);
    public void AddTaskWithoutWorker(int assignerId, int mcpId, DateTime completeByTimestamp);
    public void AssignWorkerToTask(int taskId, int workerId);
    public RequestResult FocusTask(FocusTaskRequest request);
    public RequestResult CompleteTask(CompleteTaskRequest request);
    public RequestResult RejectTask(RejectTaskRequest request);
}