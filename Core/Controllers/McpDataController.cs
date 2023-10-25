using Microsoft.AspNetCore.Mvc;
using Services.Mcps;
using Commons.Communications.Mcps;
using Commons.Endpoints;
using Commons.Types;
using Microsoft.AspNetCore.Authorization;

namespace Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class McpDataController : Controller
{
    private readonly IMcpDataService _mcpDataService;

    public McpDataController(IMcpDataService mcpDataService)
    {
        _mcpDataService = mcpDataService;
    }

    [HttpPost(Endpoints.McpData.ADD)]
    public IActionResult AddNewMcp(AddNewMcpRequest request)
    {
        var result = _mcpDataService.AddNewMcp(request);
        return ProcessRequestResult(result);
    }

    [HttpPost(Endpoints.McpData.UPDATE)]
    public IActionResult UpdateMcp(UpdateMcpRequest request)
    {
        var result = _mcpDataService.UpdateMcp(request);
        return ProcessRequestResult(result);
    }

    [HttpPost(Endpoints.McpData.REMOVE)]
    public IActionResult RemoveMcp(RemoveMcpRequest request)
    {
        var result = _mcpDataService.RemoveMcp(request);
        return ProcessRequestResult(result);
    }

    [HttpPost(Endpoints.McpData.GET)]
    public IActionResult GetMcpData(McpDataQueryParameters parameters)
    {
        var result = _mcpDataService.GetMcpData(parameters);
        return ProcessRequestResult(result);
    }
}