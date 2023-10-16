using Commons.Communications.Tasks;
using Commons.HubHandlers;
using Commons.Models;
using Commons.RequestStatuses;
using Hubs;
using Microsoft.AspNetCore.SignalR;
using Repositories.Managers;

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

    public RequestResult AddTask(AddTaskRequest request)
    {
        var taskData = new TaskData
        {
            AssignerAccountId = request.AssignerAccountId,
            AssigneeAccountId = request.AssigneeAccountId,
            McpDataId = request.McpDataId,
            AssignedTimestamp = DateTime.Now,
            IsCompleted = false
        };
        _unitOfWork.TaskDatas.Add(taskData);
        _unitOfWork.Complete();

        return new RequestResult(new Success());
    }

    public RequestResult CompleteTask(CompleteTaskRequest request)
    {
        if (!_unitOfWork.TaskDatas.DoesIdExist(request.TaskId)) return new RequestResult(new DataEntryNotFound());

        var taskData = _unitOfWork.TaskDatas.GetById(request.TaskId);
        taskData.IsCompleted = true;
        taskData.CompletedTimestamp = DateTime.Now;
        _unitOfWork.Complete();

        return new RequestResult(new Success());
    }

    public RequestResult RejectTask(RejectTaskRequest request)
    {
        if (!_unitOfWork.TaskDatas.DoesIdExist(request.TaskId)) return new RequestResult(new DataEntryNotFound());

        var taskData = _unitOfWork.TaskDatas.GetById(request.TaskId);
        taskData.IsCompleted = false;
        taskData.CompletedTimestamp = null;
        _unitOfWork.Complete();

        _hubContext.Clients.Client(BaseHub.ConnectionIds[taskData.AssignerAccountId])
            .SendAsync(HubHandlers.Tasks.REJECT_TASK, new RejectTaskBroadcastData
            {
                TaskId = taskData.Id,
            });

        return new RequestResult(new Success());
    }
}