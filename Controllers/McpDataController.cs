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
    
    [HttpPost("get")]
    public IActionResult GetMcpData(McpDataQueryParameters parameters)
    {
        var result = _mcpDataService.GetMcpData(parameters);
        return ProcessRequestResult(result);
    }
}