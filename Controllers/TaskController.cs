using Commons.Communications.Messages;
using Commons.Communications.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Requests;
using Services.Messaging;
using Services.Tasks;

namespace Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class TaskController : Controller
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpPost(Endpoints.TaskData.ADD_TASK)]
    public IActionResult AddTask(AddTaskRequest request)
    {
        var result = _taskService.AddTask(request);
        return ProcessRequestResult(result);
    }

    [HttpPost(Endpoints.TaskData.COMPLETE_TASK)]
    public IActionResult CompleteTask(CompleteTaskRequest request)
    {
        var result = _taskService.CompleteTask(request);
        return ProcessRequestResult(result);
    }
}