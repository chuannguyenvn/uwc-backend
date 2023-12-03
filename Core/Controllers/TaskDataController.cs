using Commons.Communications.Tasks;
using Commons.Endpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Tasks;

namespace Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class TaskDataController : Controller
{
    private readonly ITaskService _taskService;

    public TaskDataController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpPost(Endpoints.TaskData.GET_TASKS_OF_WORKER)]
    public IActionResult GetTasksOfWorker(GetTasksOfWorkerRequest request)
    {
        var result = _taskService.GetTasksOfWorker(request);
        return ProcessRequestResult(result);
    }

    [HttpPost(Endpoints.TaskData.GET_ALL_TASKS)]
    public IActionResult GetAllTasks()
    {
        var result = _taskService.GetAllTasks();
        return ProcessRequestResult(result);
    }

    [HttpPost(Endpoints.TaskData.ADD_TASK)]
    public IActionResult AddTask(AddTasksRequest request)
    {
        var result = _taskService.ProcessAddTaskRequest(request);
        return ProcessRequestResult(result);
    }

    [HttpPost(Endpoints.TaskData.FOCUS_TASK)]
    public IActionResult FocusTask(FocusTaskRequest request)
    {
        var result = _taskService.FocusTask(request);
        return ProcessRequestResult(result);
    }

    [HttpPost(Endpoints.TaskData.COMPLETE_TASK)]
    public IActionResult CompleteTask(CompleteTaskRequest request)
    {
        var result = _taskService.CompleteTask(request);
        return ProcessRequestResult(result);
    }

    [HttpPost(Endpoints.TaskData.REJECT_TASK)]
    public IActionResult RejectTask(RejectTaskRequest request)
    {
        var result = _taskService.RejectTask(request);
        return ProcessRequestResult(result);
    }
}