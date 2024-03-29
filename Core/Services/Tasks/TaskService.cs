﻿using Commons.Communications.Mcps;
using Commons.Communications.Tasks;
using Commons.HubHandlers;
using Commons.Models;
using Commons.RequestStatuses;
using Hubs;
using Microsoft.AspNetCore.SignalR;
using Repositories.Managers;
using Services.Mcps;
using TaskStatus = Commons.Types.TaskStatus;

namespace Services.Tasks;

public class TaskService : ITaskService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITaskOptimizationService _taskOptimizationService;
    private readonly IMcpFillLevelService _mcpFillLevelService;
    private readonly IHubContext<BaseHub> _hubContext;

    public TaskService(IUnitOfWork unitOfWork, ITaskOptimizationService taskOptimizationService, IMcpFillLevelService mcpFillLevelService,
        IHubContext<BaseHub> hubContext)
    {
        _unitOfWork = unitOfWork;
        _taskOptimizationService = taskOptimizationService;
        _mcpFillLevelService = mcpFillLevelService;
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

    public ParamRequestResult<GetTasksWithMcpResponse> GetTasksWithMcp(GetTasksWithMcpRequest request)
    {
        if (!_unitOfWork.McpDataRepository.DoesIdExist(request.McpId))
            return new ParamRequestResult<GetTasksWithMcpResponse>(new DataEntryNotFound());

        var tasks = _unitOfWork.TaskDataDataRepository.GetTasksByMcpId(request.McpId);
        return new ParamRequestResult<GetTasksWithMcpResponse>(new Success(), new GetTasksWithMcpResponse
        {
            Tasks = tasks
        });
    }

    public ParamRequestResult<GetWorkerPrioritizedTaskResponse> GetWorkerPrioritizedTask(GetWorkerPrioritizedTaskRequest request)
    {
        if (!_unitOfWork.AccountRepository.DoesIdExist(request.WorkerId))
            return new ParamRequestResult<GetWorkerPrioritizedTaskResponse>(new DataEntryNotFound());

        var task = _unitOfWork.TaskDataDataRepository.GetPrioritizedTaskByWorkerId(request.WorkerId);
        return new ParamRequestResult<GetWorkerPrioritizedTaskResponse>(new Success(), new GetWorkerPrioritizedTaskResponse
        {
            Task = task
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

        if (request.AssigneeAccountId.HasValue && BaseHub.ConnectionIds.TryGetValue(request.AssigneeAccountId.Value, out var assigneeConnectionId))
        {
            _hubContext.Clients.Client(assigneeConnectionId)
                .SendAsync(HubHandlers.Tasks.ADD_TASK, new AddTasksBroadcastData
                {
                    NewTasks = _unitOfWork.TaskDataDataRepository.GetTasksByWorkerId(request.AssigneeAccountId.Value).ToList()
                });
        }

        if (BaseHub.ConnectionIds.TryGetValue(request.AssignerAccountId, out var assignerConnectionId))
        {
            _hubContext.Clients.Client(assignerConnectionId)
                .SendAsync(HubHandlers.Tasks.ADD_TASK, new AddTasksBroadcastData
                {
                    NewTasks = _unitOfWork.TaskDataDataRepository.GetTasksFromTodayOrFuture(),
                });
        }

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
            task.LastStatusChangeTimestamp = DateTime.UtcNow;
        }

        var taskData = _unitOfWork.TaskDataDataRepository.GetById(request.TaskId);
        taskData.TaskStatus = TaskStatus.InProgress;
        taskData.LastStatusChangeTimestamp = DateTime.UtcNow;
        _unitOfWork.Complete();

        if (BaseHub.ConnectionIds.TryGetValue(taskData.AssignerId, out var connectionId))
        {
            _hubContext.Clients.Client(connectionId)
                .SendAsync(HubHandlers.Tasks.FOCUS_TASK, new FocusTaskBroadcastData()
                {
                    TaskId = taskData.Id,
                    WorkerId = taskData.AssigneeId ?? -1,
                });
        }

        return new RequestResult(new Success());
    }

    public RequestResult CompleteTask(CompleteTaskRequest request)
    {
        if (!_unitOfWork.TaskDataDataRepository.DoesIdExist(request.TaskId)) return new RequestResult(new DataEntryNotFound());

        var taskData = _unitOfWork.TaskDataDataRepository.GetById(request.TaskId);
        taskData.TaskStatus = TaskStatus.Completed;
        taskData.LastStatusChangeTimestamp = DateTime.UtcNow;
        _unitOfWork.Complete();

        _mcpFillLevelService.EmptyMcp(new EmptyMcpRequest
        {
            McpId = taskData.McpDataId,
            WorkerId = taskData.AssigneeId.Value,
        });

        if (BaseHub.ConnectionIds.TryGetValue(taskData.AssignerId, out var connectionId))
        {
            _hubContext.Clients.Client(connectionId)
                .SendAsync(HubHandlers.Tasks.COMPLETE_TASK, new CompleteTaskBroadcastData
                {
                    TaskId = taskData.Id,
                });
        }

        return new RequestResult(new Success());
    }

    public RequestResult RejectTask(RejectTaskRequest request)
    {
        if (!_unitOfWork.TaskDataDataRepository.DoesIdExist(request.TaskId)) return new RequestResult(new DataEntryNotFound());

        var taskData = _unitOfWork.TaskDataDataRepository.GetById(request.TaskId);
        taskData.TaskStatus = TaskStatus.Rejected;
        taskData.LastStatusChangeTimestamp = DateTime.UtcNow;
        _unitOfWork.Complete();

        if (BaseHub.ConnectionIds.TryGetValue(taskData.AssignerId, out var connectionId))
        {
            _hubContext.Clients.Client(connectionId)
                .SendAsync(HubHandlers.Tasks.REJECT_TASK, new RejectTaskBroadcastData
                {
                    TaskId = taskData.Id,
                });
        }

        return new RequestResult(new Success());
    }
}