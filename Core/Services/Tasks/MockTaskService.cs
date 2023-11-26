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
        AddTask(addTaskRequest);
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
        AddTask(addTaskRequest);
    }

    public RequestResult AddTask(AddTasksRequest request)
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

    public RequestResult AssignWorkerToTask(AssignWorkerToTaskRequest request)
    {
        if (!_unitOfWork.TaskDataDataRepository.DoesIdExist(request.TaskId)) return new RequestResult(new DataEntryNotFound());
        if (!_unitOfWork.AccountRepository.DoesIdExist(request.WorkerId)) return new RequestResult(new DataEntryNotFound());

        var taskData = _unitOfWork.TaskDataDataRepository.GetById(request.TaskId);

        if (taskData.AssigneeId.HasValue) return new RequestResult(new DataEntryAlreadyExist());
        
        taskData.AssigneeId = request.WorkerId;
        taskData.AssigneeProfile = _unitOfWork.UserProfileRepository.GetById(request.WorkerId);
        _unitOfWork.Complete();

        return new RequestResult(new Success());
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