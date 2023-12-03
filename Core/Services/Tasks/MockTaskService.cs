using Commons.Communications.Tasks;
using Commons.Models;
using Commons.RequestStatuses;
using Repositories.Managers;
using TaskStatus = Commons.Types.TaskStatus;

namespace Services.Tasks;

public class MockTaskService : ITaskService
{
    private readonly IUnitOfWork _unitOfWork;

    public MockTaskService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public ParamRequestResult<GetTasksOfWorkerResponse> GetTasksOfWorker(GetTasksOfWorkerRequest request)
    {
        throw new NotImplementedException();
    }

    public ParamRequestResult<GetAllTasksResponse> GetAllTasks()
    {
        throw new NotImplementedException();
    }

    public void AddTaskWithWorker(int supervisorId, int workerId, int mcpId, DateTime deadline)
    {
        var addTaskRequest = new AddTasksRequest
        {
            AssignerAccountId = supervisorId,
            AssigneeAccountId = workerId,
            McpDataIds = new List<int>() { mcpId },
            CompleteByTimestamp = deadline,
        };
        ProcessAddTaskRequest(addTaskRequest);
    }

    public void AddTaskWithoutWorker(int supervisorId, int mcpId, DateTime deadline)
    {
        var addTaskRequest = new AddTasksRequest
        {
            AssignerAccountId = supervisorId,
            AssigneeAccountId = null,
            McpDataIds = new List<int>() { mcpId },
            CompleteByTimestamp = deadline,
        };
        ProcessAddTaskRequest(addTaskRequest);
    }

    public RequestResult ProcessAddTaskRequest(AddTasksRequest request)
    {
        foreach (var mcpDataId in request.McpDataIds)
        {
            var taskData = new TaskData
            {
                AssignerId = request.AssignerAccountId,
                AssigneeId = request.AssigneeAccountId,
                McpDataId = mcpDataId,
                CreatedTimestamp = DateTime.Now,
                CompleteByTimestamp = request.CompleteByTimestamp,
                TaskStatus = TaskStatus.NotStarted,
            };
            _unitOfWork.TaskDataDataRepository.Add(taskData);
        }

        _unitOfWork.Complete();

        return new RequestResult(new Success());
    }

    public void AssignWorkerToTask(int taskId, int workerId)
    {
        if (!_unitOfWork.TaskDataDataRepository.DoesIdExist(taskId)) throw new Exception("Task not found");
        if (!_unitOfWork.AccountRepository.DoesIdExist(workerId)) throw new Exception("Worker not found");

        var taskData = _unitOfWork.TaskDataDataRepository.GetById(taskId);

        if (taskData.AssigneeId.HasValue) throw new Exception("Task already has a worker assigned");

        taskData.AssigneeId = workerId;
        _unitOfWork.Complete();
    }

    public RequestResult FocusTask(FocusTaskRequest request)
    {
        throw new NotImplementedException();
    }

    public RequestResult CompleteTask(CompleteTaskRequest request)
    {
        throw new NotImplementedException();
    }

    public RequestResult RejectTask(RejectTaskRequest request)
    {
        throw new NotImplementedException();
    }
}