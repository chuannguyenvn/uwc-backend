﻿using Commons.Communications.Messages;
using Microsoft.AspNetCore.Mvc;
using Services.Messaging;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class MessageController : Controller
{
    private readonly IMessagingService _messagingService;

    public MessageController(IMessagingService messagingService)
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