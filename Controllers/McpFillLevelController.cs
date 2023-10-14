using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Mcps;

namespace Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class McpFillLevelController : Controller
{
    private readonly IMcpFillLevelService _mcpFillLevelService;

    public McpFillLevelController(IMcpFillLevelService mcpFillLevelService)
    {
        _mcpFillLevelService = mcpFillLevelService;
    }
    
    [HttpPost("get-all")]
    public IActionResult GetAllFillLevel()
    {
        var result = _mcpFillLevelService.GetAllFillLevel();
        return ProcessRequestResult(result);
    }
}