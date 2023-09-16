using Microsoft.AspNetCore.Mvc;
using Services.Mcps;
using Commons.Communications.Mcps;
using Commons.Types;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class McpDataController : Controller
{
    private readonly IMcpDataService _mcpDataService;

    public McpDataController(IMcpDataService mcpDataService)
    {
        _mcpDataService = mcpDataService;
    }

    [HttpPost("add")]
    public IActionResult AddNewMcp(AddNewMcpRequest request)
    {
        var result = _mcpDataService.AddNewMcp(request);
        return ProcessRequestResult(result);
    }

    [HttpPost("update")]
    public IActionResult UpdateMcp(UpdateMcpRequest request)
    {
        var result = _mcpDataService.UpdateMcp(request);
        return ProcessRequestResult(result);
    }

    [HttpPost("remove")]
    public IActionResult RemoveMcp(RemoveMcpRequest request)
    {
        var result = _mcpDataService.RemoveMcp(request);
        return ProcessRequestResult(result);
    }

    [HttpGet("get/stable")]
    public IActionResult GetAllStableData(McpQueryParameters parameters)
    {
        var result = _mcpDataService.GetAllStableData(parameters);
        return ProcessRequestResult(result);
    }

    [HttpGet("get/volatile/{pageId}")]
    public IActionResult GetAllVolatileData(McpQueryParameters parameters)
    {
        var result = _mcpDataService.GetAllVolatileData(parameters);
        return ProcessRequestResult(result);
    }
}