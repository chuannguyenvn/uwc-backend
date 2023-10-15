using Commons.Communications.Tasks;
using Commons.Models;
using Commons.RequestStatuses;
using Repositories.Managers;

namespace Services.Tasks;

public class TaskService : ITaskService
{
    private readonly IUnitOfWork _unitOfWork;

    public TaskService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public RequestResult AddTask(AddTaskRequest request)
    {
        var taskData = new TaskData
        {
            AssignerAccountId = request.AssignerAccountId,
            AssigneeAccountId = request.AssigneeAccountId,
            McpDataId = request.McpDataId,
            Timestamp = DateTime.Now,
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
        _unitOfWork.Complete();

        return new RequestResult(new Success());
    }
}