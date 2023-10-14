using Microsoft.AspNetCore.Mvc;
using Services.Messaging;
using Commons.Communications.Messages;
using Microsoft.AspNetCore.Authorization;

namespace Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class MessagingController : Controller
{
    private readonly IMessagingService _messagingService;

    public MessagingController(IMessagingService messagingService)
    {
        _messagingService = messagingService;
    }

    [HttpPost("send")]
    public IActionResult SendMessage(SendMessageRequest request)
    {
        var result = _messagingService.SendMessage(request);
        return ProcessRequestResult(result);
    }
}