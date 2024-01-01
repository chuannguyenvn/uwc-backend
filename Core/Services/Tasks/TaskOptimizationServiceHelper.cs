using Commons.Communications.Map;
using Commons.Communications.Tasks;
using Commons.HubHandlers;
using Commons.Models;
using Commons.Types;
using Hubs;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Repositories.Managers;
using Services.Map;
using Services.Mcps;
using Services.Status;
using TaskStatus = Commons.Types.TaskStatus;

namespace Services.Tasks;

public class TaskOptimizationServiceHelper
{
    private static int CurrentTaskGroupId = 0;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IHubContext<BaseHub> _hubContext;
    private readonly ILocationService _locationService;
    private readonly IMcpFillLevelService _mcpFillLevelService;
    private readonly IOnlineStatusService _onlineStatusService;

    public TaskOptimizationServiceHelper(IUnitOfWork unitOfWork, IHubContext<BaseHub> hubContext, ILocationService locationService,
        IMcpFillLevelService mcpFillLevelService, IOnlineStatusService onlineStatusService)
    {
        CurrentTaskGroupId = unitOfWork.TaskDataDataRepository.GetMaxTaskGroupId();

        _unitOfWork = unitOfWork;
        _hubContext = hubContext;
        _locationService = locationService;
        _mcpFillLevelService = mcpFillLevelService;
        _onlineStatusService = onlineStatusService;
    }

    public List<TaskData> GetWorkerTasksIn24Hours(int workerId)
    {
        return _unitOfWork.TaskDataDataRepository.GetWorkerRemainingTasksIn24Hours(workerId);
    }

    public List<TaskData> GetUnassignedTaskIn24Hours()
    {
        return _unitOfWork.TaskDataDataRepository.GetUnassignedTasksIn24Hours();
    }

    public Dictionary<int, float> GetAllMcpFillLevels()
    {
        return _mcpFillLevelService.GetAllFillLevel().Data.FillLevelsById;
    }
    
    public TaskData? GetNextMcpTask(int mcpId)
    {
        return _unitOfWork.TaskDataDataRepository.GetNextMcpTask(mcpId);
    }

    public Coordinate GetMcpCoordinateById(int mcpId)
    {
        return _unitOfWork.McpDataRepository.GetById(mcpId).Coordinate;
    }

    public Coordinate GetWorkerLocation(int workerId)
    {
        return _locationService.GetLocation(new GetLocationRequest
        {
            AccountId = workerId
        }).Data!.Coordinate;
    }

    public Dictionary<int, UserProfile> GetAllWorkerProfiles()
    {
        return _unitOfWork.UserProfileRepository.GetAllWorkers().ToDictionary(workerProfile => workerProfile.Id);
    }

    public bool IsWorkerOnline(int workerId)
    {
        return _onlineStatusService.IsAccountOnline(workerId);
    }

    private string ConstructMapboxMatrixRequest(List<Coordinate> coordinates)
    {
        return string.Format(Constants.MAPBOX_MATRIX_API, String.Join(';', coordinates.Select(location => location.ToStringApi())));
    }

    public MapboxMatrixResponse RequestMapboxMatrix(List<Coordinate> coordinates)
    {
        var client = new HttpClient();
        var httpResponse = client.GetStringAsync(ConstructMapboxMatrixRequest(coordinates)).Result;
        var mapboxMatrixResponse = JsonConvert.DeserializeObject<MapboxMatrixResponse>(httpResponse);
        return mapboxMatrixResponse;
    }

    public void AddTaskWithWorker(int assignerId, int workerId, int mcpId, DateTime completeByTimestamp)
    {
        CurrentTaskGroupId++;

        var taskData = new TaskData
        {
            AssignerId = assignerId,
            AssigneeId = workerId,
            McpDataId = mcpId,
            GroupId = CurrentTaskGroupId,
            CreatedTimestamp = DateTime.UtcNow,
            CompleteByTimestamp = completeByTimestamp,
            TaskStatus = TaskStatus.NotStarted,
        };
        _unitOfWork.TaskDataDataRepository.Add(taskData);

        _unitOfWork.Complete();

        if (BaseHub.ConnectionIds.TryGetValue(workerId, out var connectionId))
        {
            _hubContext.Clients.Client(connectionId)
                .SendAsync(HubHandlers.Tasks.ADD_TASK, new AddTasksBroadcastData
                {
                    NewTasks = _unitOfWork.TaskDataDataRepository.GetTasksByWorkerId(workerId).ToList()
                });
        }
    }

    public void AddTasksWithWorker(int assignerId, int workerId, List<int> mcpIds, DateTime completeByTimestamp, bool isGrouped)
    {
        CurrentTaskGroupId++;

        foreach (var mcpId in mcpIds)
        {
            var taskData = new TaskData
            {
                AssignerId = assignerId,
                AssigneeId = workerId,
                McpDataId = mcpId,
                GroupId = CurrentTaskGroupId,
                CreatedTimestamp = DateTime.UtcNow,
                CompleteByTimestamp = completeByTimestamp,
                TaskStatus = TaskStatus.NotStarted,
            };
            
            _unitOfWork.TaskDataDataRepository.Add(taskData);

            if (isGrouped is false)
            {
                CurrentTaskGroupId++;
            }
        }

        if (isGrouped is false)
        {
            CurrentTaskGroupId--;
        }
        
        _unitOfWork.Complete();

        if (BaseHub.ConnectionIds.TryGetValue(workerId, out var connectionId))
        {
            _hubContext.Clients.Client(connectionId)
                .SendAsync(HubHandlers.Tasks.ADD_TASK, new AddTasksBroadcastData
                {
                    NewTasks = _unitOfWork.TaskDataDataRepository.GetTasksByWorkerId(workerId).ToList()
                });
        }
    }

    public void AddTaskWithoutWorker(int assignerId, int mcpId, DateTime completeByTimestamp)
    {
        var taskData = new TaskData
        {
            AssignerId = assignerId,
            McpDataId = mcpId,
            GroupId = CurrentTaskGroupId,
            CreatedTimestamp = DateTime.UtcNow,
            CompleteByTimestamp = completeByTimestamp,
            TaskStatus = TaskStatus.NotStarted,
        };
        _unitOfWork.TaskDataDataRepository.Add(taskData);

        _unitOfWork.Complete();
    }

    public void AddTasksWithoutWorker(int assignerId, List<int> mcpIds, DateTime completeByTimestamp, bool isGrouped)
    {
        CurrentTaskGroupId++;

        foreach (var mcpId in mcpIds)
        {
            var taskData = new TaskData
            {
                AssignerId = assignerId,
                McpDataId = mcpId,
                GroupId = CurrentTaskGroupId,
                CreatedTimestamp = DateTime.UtcNow,
                CompleteByTimestamp = completeByTimestamp,
                TaskStatus = TaskStatus.NotStarted,
            };

            _unitOfWork.TaskDataDataRepository.Add(taskData);
        }

        _unitOfWork.Complete();
    }

    public void UpdatePriority(int taskId, int priority)
    {
        if (!_unitOfWork.TaskDataDataRepository.DoesIdExist(taskId)) throw new Exception("Task not found");

        var taskData = _unitOfWork.TaskDataDataRepository.GetById(taskId);
        taskData.Priority = priority;
        _unitOfWork.TaskDataDataRepository.Update(taskData);

        _unitOfWork.Complete();
    }

    public void AutoAssignPriorities(List<int> taskIds)
    {
        for (var i = 0; i < taskIds.Count; i++)
        {
            UpdatePriority(taskIds[i], i);
        }
    }

    public void AssignWorkerToTask(int taskId, int workerId)
    {
        if (!_unitOfWork.TaskDataDataRepository.DoesIdExist(taskId)) throw new Exception("Task not found");
        if (!_unitOfWork.AccountRepository.DoesIdExist(workerId)) throw new Exception("Worker not found");

        var taskData = _unitOfWork.TaskDataDataRepository.GetById(taskId);

        if (taskData.AssigneeId.HasValue) throw new Exception("Task already has a worker assigned");

        taskData.AssigneeId = workerId;
        _unitOfWork.TaskDataDataRepository.Update(taskData);
        _unitOfWork.Complete();

        if (BaseHub.ConnectionIds.TryGetValue(workerId, out var connectionId))
        {
            _hubContext.Clients.Client(connectionId)
                .SendAsync(HubHandlers.Tasks.ADD_TASK, new AddTasksBroadcastData
                {
                    NewTasks = _unitOfWork.TaskDataDataRepository.GetTasksByWorkerId(workerId).ToList()
                });
        }
    }

    private void SwapLists(List<List<List<int>>> list, int index1, int index2)
    {
        if (index1 < 0 || index1 >= list.Count)
        {
            throw new ArgumentOutOfRangeException("Index 1 out of range");
        }

        if (index2 < 0 || index2 >= list.Count)
        {
            throw new ArgumentOutOfRangeException("Index 2 out of range");
        }

        (list[index1], list[index2]) = (list[index2], list[index1]);
    }

    private int CompareListsInner(List<int> list1, List<int> list2)
    {
        for (int i = 0; i < Math.Min(list1.Count, list2.Count); i++)
        {
            int result = list1[i].CompareTo(list2[i]);

            if (result != 0)
            {
                return result;
            }
        }

        return list1.Count.CompareTo(list2.Count);
    }

    private int CompareLists(List<List<int>> list1, List<List<int>> list2)
    {
        for (int i = 0; i < Math.Min(list1.Count, list2.Count); i++)
        {
            int result = CompareListsInner(list1[i], list2[i]);

            if (result != 0)
            {
                return result;
            }
        }

        return list1.Count.CompareTo(list2.Count);
    }

    public bool NextPermutation(List<List<List<int>>> arr)
    {
        int index1 = arr.Count - 1;
        while (index1 > 0 && CompareLists(arr[index1 - 1], arr[index1]) >= 0)
        {
            index1--;
        }

        if (index1 <= 0)
        {
            // return false;
            arr.Reverse();
            return true;
        }

        int index2 = arr.Count - 1;
        while (CompareLists(arr[index2], arr[index1 - 1]) <= 0)
        {
            index2--;
        }

        SwapLists(arr, index1 - 1, index2);

        index2 = arr.Count - 1;
        while (index1 < index2)
        {
            SwapLists(arr, index1, index2);
            index1++;
            index2--;
        }

        return true;
    }

    public McpData GetMcpDataById(int mcpId)
    {
        return _unitOfWork.McpDataRepository.GetById(mcpId);
    }
}