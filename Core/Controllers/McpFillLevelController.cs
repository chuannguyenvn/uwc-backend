using Commons.Communications.Mcps;
using Commons.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Services.Mcps;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class McpFillLevelController : Controller
{
    private readonly IMcpFillLevelService _mcpFillLevelService;

    public McpFillLevelController(IMcpFillLevelService mcpFillLevelService)
    {
        _mcpFillLevelService = mcpFillLevelService;
    }
    
    [HttpPost(Endpoints.McpFillLevel.GET_ALL)]
    public IActionResult GetAllFillLevel()
    {
        var result = _mcpFillLevelService.GetAllFillLevel();
        return ProcessRequestResult(result);
    }
    
    [HttpPost(Endpoints.McpFillLevel.GET)]
    public IActionResult GetFillLevel(GetFillLevelRequest request)
    {
        var result = _mcpFillLevelService.GetFillLevel(request);
        return ProcessRequestResult(result);
    }
    
    [HttpPost(Endpoints.McpFillLevel.SET)]
    public IActionResult SetFillLevel(SetFillLevelRequest request)
    {
        var result = _mcpFillLevelService.SetFillLevel(request);
        return ProcessRequestResult(result);
    }
    
    [HttpPost(Endpoints.McpFillLevel.EMPTY)]
    public IActionResult EmptyMcp(EmptyMcpRequest request)
    {
        var result = _mcpFillLevelService.EmptyMcp(request);
        return ProcessRequestResult(result);
    }
}