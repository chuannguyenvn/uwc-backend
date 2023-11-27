using Commons.Communications.Status;
using Commons.Endpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Status;

namespace Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class StatusController : Controller
{
    private readonly IWorkingStatusService _workingStatusService;

    public StatusController(IWorkingStatusService workingStatusService)
    {
        _workingStatusService = workingStatusService;
    }

    [HttpPost(Endpoints.Status.GET_WORKING_STATUS)]
    public IActionResult GetWorkingStatus(GetWorkingStatusRequest request)
    {
        var result = _workingStatusService.GetWorkingStatus(request);
        return ProcessRequestResult(result);
    }
}