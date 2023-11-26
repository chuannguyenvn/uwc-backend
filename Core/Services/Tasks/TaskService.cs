using Commons.Communications.Tasks;
using Commons.HubHandlers;
using Commons.Models;
using Commons.RequestStatuses;
using Hubs;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
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
        Console.WriteLine(JsonConvert.SerializeObject(_unitOfWork.TaskDataDataRepository.GetTasksFromTodayOrFuture()));

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
        Console.WriteLine(JsonConvert.SerializeObject(_unitOfWork.TaskDataDataRepository.GetTasksFromTodayOrFuture()));

        return new ParamRequestResult<GetAllTasksResponse>(new Success(), new GetAllTasksResponse()
        {
            Tasks = _unitOfWork.TaskDataDataRepository.GetTasksFromTodayOrFuture(),
        });
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

        if (request.AssigneeAccountId.HasValue)
            _hubContext.Clients.Client(BaseHub.ConnectionIds[request.AssigneeAccountId.Value])
                .SendAsync(HubHandlers.Tasks.ADD_TASK, new AddTasksBroadcastData
                {
                    NewTasks = _unitOfWork.TaskDataDataRepository.GetTasksByWorkerId(request.AssigneeAccountId.Value).ToList()
                });

        return new RequestResult(new Success());
    }

    public RequestResult AssignWorkerToTask(AssignWorkerToTaskRequest request)
    {
        if (!_unitOfWork.TaskDataDataRepository.DoesIdExist(request.TaskId)) return new RequestResult(new DataEntryNotFound());
        if (!_unitOfWork.AccountRepository.DoesIdExist(request.WorkerId)) return new RequestResult(new DataEntryNotFound());

        var taskData = _unitOfWork.TaskDataDataRepository.GetById(request.TaskId);

        if (taskData.AssigneeId.HasValue) return new RequestResult(new DataEntryAlreadyExist());

        taskData.AssigneeId = request.WorkerId;
        _unitOfWork.Complete();

        _hubContext.Clients.Client(BaseHub.ConnectionIds[request.WorkerId])
            .SendAsync(HubHandlers.Tasks.ADD_TASK, new AddTasksBroadcastData
            {
                NewTasks = _unitOfWork.TaskDataDataRepository.GetTasksByWorkerId(request.WorkerId).ToList()
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