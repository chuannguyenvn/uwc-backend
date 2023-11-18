using Commons.Communications.Tasks;
using Commons.HubHandlers;
using Commons.Models;
using Commons.RequestStatuses;
using Hubs;
using Microsoft.AspNetCore.SignalR;
using Repositories.Managers;
using TaskStatus = Commons.Types.TaskStatus;

namespace Services.Tasks;

public class TaskService : ITaskService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHubContext<BaseHub> _hubContext;

    public TaskService(IUnitOfWork unitOfWork, IHubContext<BaseHub> hubContext)
    {
        _unitOfWork = unitOfWork;
        _hubContext = hubContext;
    }

    public ParamRequestResult<GetTasksOfWorkerResponse> GetTasksOfWorker(GetTasksOfWorkerRequest request)
    {
        if (!_unitOfWork.AccountRepository.DoesIdExist(request.WorkerId))
            return new ParamRequestResult<GetTasksOfWorkerResponse>(new DataEntryNotFound());

        var tasks = _unitOfWork.TaskDataRepository.GetTasksByWorkerId(request.WorkerId);
        return new ParamRequestResult<GetTasksOfWorkerResponse>(new Success(), new GetTasksOfWorkerResponse
        {
            Tasks = tasks
        });
    }

    public ParamRequestResult<GetAllTasksResponse> GetAllTasks()
    {
        return new ParamRequestResult<GetAllTasksResponse>(new Success(), new GetAllTasksResponse()
        {
            Tasks = _unitOfWork.TaskDataRepository.GetTasksFromTodayOrFuture(),
        });
    }

    public RequestResult AddTask(AddTasksRequest request)
    {
        foreach (var mcpDataId in request.McpDataIds)
        {
            var taskData = new TaskData
            {
                AssignerAccountId = request.AssignerAccountId,
                AssigneeAccountId = request.AssigneeAccountId,
                McpDataId = mcpDataId,
                CreatedTimestamp = DateTime.Now,
                CompleteByTimestamp = request.CompleteByTimestamp,
                TaskStatus = TaskStatus.NotStarted,
            };
            _unitOfWork.TaskDataRepository.Add(taskData);
        }

        _unitOfWork.Complete();

        _hubContext.Clients.Client(BaseHub.ConnectionIds[request.AssigneeAccountId])
            .SendAsync(HubHandlers.Tasks.ADD_TASK, new AddTasksBroadcastData
            {
                NewTasks = _unitOfWork.TaskDataRepository.GetTasksByWorkerId(request.AssigneeAccountId).ToList()
            });

        return new RequestResult(new Success());
    }

    public RequestResult FocusTask(FocusTaskRequest request)
    {
        if (!_unitOfWork.TaskDataRepository.DoesIdExist(request.TaskId)) return new RequestResult(new DataEntryNotFound());

        var workerTasks = _unitOfWork.TaskDataRepository.GetTasksByWorkerId(request.WorkerId);
        foreach (var task in workerTasks)
        {
            if (task.TaskStatus != TaskStatus.InProgress) continue;

            task.TaskStatus = TaskStatus.NotStarted;
            task.LastStatusChangeTimestamp = DateTime.Now;
        }

        var taskData = _unitOfWork.TaskDataRepository.GetById(request.TaskId);
        taskData.TaskStatus = TaskStatus.InProgress;
        taskData.LastStatusChangeTimestamp = DateTime.Now;
        _unitOfWork.Complete();

        return new RequestResult(new Success());
    }

    public RequestResult CompleteTask(CompleteTaskRequest request)
    {
        if (!_unitOfWork.TaskDataRepository.DoesIdExist(request.TaskId)) return new RequestResult(new DataEntryNotFound());

        var taskData = _unitOfWork.TaskDataRepository.GetById(request.TaskId);
        taskData.TaskStatus = TaskStatus.Completed;
        taskData.LastStatusChangeTimestamp = DateTime.Now;
        _unitOfWork.Complete();

        _hubContext.Clients.Client(BaseHub.ConnectionIds[taskData.AssignerAccountId])
            .SendAsync(HubHandlers.Tasks.COMPLETE_TASK, new CompleteTaskBroadcastData
            {
                TaskId = taskData.Id,
            });

        return new RequestResult(new Success());
    }

    public RequestResult RejectTask(RejectTaskRequest request)
    {
        if (!_unitOfWork.TaskDataRepository.DoesIdExist(request.TaskId)) return new RequestResult(new DataEntryNotFound());

        var taskData = _unitOfWork.TaskDataRepository.GetById(request.TaskId);
        taskData.TaskStatus = TaskStatus.Rejected;
        taskData.LastStatusChangeTimestamp = DateTime.Now;
        _unitOfWork.Complete();

        _hubContext.Clients.Client(BaseHub.ConnectionIds[taskData.AssignerAccountId])
            .SendAsync(HubHandlers.Tasks.REJECT_TASK, new RejectTaskBroadcastData
            {
                TaskId = taskData.Id,
            });

        return new RequestResult(new Success());
    }
}