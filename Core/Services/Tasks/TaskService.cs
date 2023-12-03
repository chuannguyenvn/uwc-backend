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
    private readonly ITaskOptimizationService _taskOptimizationService;
    private readonly IHubContext<BaseHub> _hubContext;

    public TaskService(IUnitOfWork unitOfWork, ITaskOptimizationService taskOptimizationService, IHubContext<BaseHub> hubContext)
    {
        _unitOfWork = unitOfWork;
        _taskOptimizationService = taskOptimizationService;
        _hubContext = hubContext;
    }

    public ParamRequestResult<GetTasksOfWorkerResponse> GetTasksOfWorker(GetTasksOfWorkerRequest request)
    {
        if (!_unitOfWork.AccountRepository.DoesIdExist(request.WorkerId))
            return new ParamRequestResult<GetTasksOfWorkerResponse>(new DataEntryNotFound());

        var tasks = _unitOfWork.TaskDataDataRepository.GetTasksByWorkerId(request.WorkerId);
        return new ParamRequestResult<GetTasksOfWorkerResponse>(new Success(), new GetTasksOfWorkerResponse
        {
            Tasks = tasks
        });
    }

    public ParamRequestResult<GetAllTasksResponse> GetAllTasks()
    {
        return new ParamRequestResult<GetAllTasksResponse>(new Success(), new GetAllTasksResponse()
        {
            Tasks = _unitOfWork.TaskDataDataRepository.GetTasksFromTodayOrFuture(),
        });
    }

    public RequestResult AddTask(AddTasksRequest request)
    {
        _taskOptimizationService.ProcessAddTaskRequest(request);
        
        if (request.AssigneeAccountId.HasValue)
            _hubContext.Clients.Client(BaseHub.ConnectionIds[request.AssigneeAccountId.Value])
                .SendAsync(HubHandlers.Tasks.ADD_TASK, new AddTasksBroadcastData
                {
                    NewTasks = _unitOfWork.TaskDataDataRepository.GetTasksByWorkerId(request.AssigneeAccountId.Value).ToList()
                });

        return new RequestResult(new Success());
    }

    public RequestResult FocusTask(FocusTaskRequest request)
    {
        if (!_unitOfWork.TaskDataDataRepository.DoesIdExist(request.TaskId)) return new RequestResult(new DataEntryNotFound());

        var workerTasks = _unitOfWork.TaskDataDataRepository.GetTasksByWorkerId(request.WorkerId);
        foreach (var task in workerTasks)
        {
            if (task.TaskStatus != TaskStatus.InProgress) continue;

            task.TaskStatus = TaskStatus.NotStarted;
            task.LastStatusChangeTimestamp = DateTime.Now;
        }

        var taskData = _unitOfWork.TaskDataDataRepository.GetById(request.TaskId);
        taskData.TaskStatus = TaskStatus.InProgress;
        taskData.LastStatusChangeTimestamp = DateTime.Now;
        _unitOfWork.Complete();

        return new RequestResult(new Success());
    }

    public RequestResult CompleteTask(CompleteTaskRequest request)
    {
        if (!_unitOfWork.TaskDataDataRepository.DoesIdExist(request.TaskId)) return new RequestResult(new DataEntryNotFound());

        var taskData = _unitOfWork.TaskDataDataRepository.GetById(request.TaskId);
        taskData.TaskStatus = TaskStatus.Completed;
        taskData.LastStatusChangeTimestamp = DateTime.Now;
        _unitOfWork.Complete();

        _hubContext.Clients.Client(BaseHub.ConnectionIds[taskData.AssignerId])
            .SendAsync(HubHandlers.Tasks.COMPLETE_TASK, new CompleteTaskBroadcastData
            {
                TaskId = taskData.Id,
            });

        return new RequestResult(new Success());
    }

    public RequestResult RejectTask(RejectTaskRequest request)
    {
        if (!_unitOfWork.TaskDataDataRepository.DoesIdExist(request.TaskId)) return new RequestResult(new DataEntryNotFound());

        var taskData = _unitOfWork.TaskDataDataRepository.GetById(request.TaskId);
        taskData.TaskStatus = TaskStatus.Rejected;
        taskData.LastStatusChangeTimestamp = DateTime.Now;
        _unitOfWork.Complete();

        _hubContext.Clients.Client(BaseHub.ConnectionIds[taskData.AssignerId])
            .SendAsync(HubHandlers.Tasks.REJECT_TASK, new RejectTaskBroadcastData
            {
                TaskId = taskData.Id,
            });

        return new RequestResult(new Success());
    }
}