using Commons.Communications.Messages;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class MessageController : ControllerBase
{
    [HttpPost("send")]
    public IActionResult SendMessage(SendMessageRequest request)
    {
        return Ok();
    }
}