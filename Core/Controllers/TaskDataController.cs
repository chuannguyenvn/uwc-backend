using Commons.Communications.Tasks;
using Commons.Endpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Tasks;

namespace Controllers;

// [Authorize]
[ApiController]
[Route("[controller]")]
public class TaskDataController : Controller
{
    private readonly ITaskService _taskService;
    private readonly ITaskOptimizationService _taskOptimizationService;

    public TaskDataController(ITaskService taskService, ITaskOptimizationService taskOptimizationService)
    {
        _taskService = taskService;
        _taskOptimizationService = taskOptimizationService;
    }

    [HttpPost(Endpoints.TaskData.GET_TASKS_OF_WORKER)]
    public IActionResult GetTasksOfWorker(GetTasksOfWorkerRequest request)
    {
        var result = _taskService.GetTasksOfWorker(request);
        return ProcessRequestResult(result);
    }

    [HttpPost(Endpoints.TaskData.GET_TASKS_WITH_MCP)]
    public IActionResult GetTasksWithMcp(GetTasksWithMcpRequest request)
    {
        var result = _taskService.GetTasksWithMcp(request);
        return ProcessRequestResult(result);
    }

    [HttpPost(Endpoints.TaskData.GET_ALL_TASKS)]
    public IActionResult GetAllTasks()
    {
        var result = _taskService.GetAllTasks();
        return ProcessRequestResult(result);
    }

    [HttpPost(Endpoints.TaskData.GET_WORKER_PRIORITIZED_TASK)]
    public IActionResult GetWorkerPrioritizedTask(GetWorkerPrioritizedTaskRequest request)
    {
        var result = _taskService.GetWorkerPrioritizedTask(request);
        return ProcessRequestResult(result);
    }

    [HttpPost(Endpoints.TaskData.ADD_TASK)]
    public IActionResult AddTask(AddTasksRequest request)
    {
        var result = _taskService.AddTask(request);
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

    [HttpPost(Endpoints.TaskData.TOGGLE_AUTO_TASK_DISTRIBUTION)]
    public IActionResult ToggleAutoTaskDistribution(ToggleAutoTaskDistributionRequest request)
    {
        var result = _taskOptimizationService.ToggleAutoTaskDistribution(request);
        return ProcessRequestResult(result);
    }
}