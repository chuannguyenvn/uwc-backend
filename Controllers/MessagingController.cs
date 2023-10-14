using Microsoft.AspNetCore.Mvc;
using Services.Messaging;
using Commons.Communications.Messages;
using Microsoft.AspNetCore.Authorization;
using Requests;

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

    [HttpPost(Endpoints.Messaging.SEND_MESSAGE)]
    public IActionResult SendMessage(SendMessageRequest request)
    {
        var result = _messagingService.SendMessage(request);
        return ProcessRequestResult(result);
    }

    [HttpPost(Endpoints.Messaging.GET_MESSAGES_BETWEEN_TWO_USERS)]
    public IActionResult GetMessagesBetweenTwoUsers(GetMessagesBetweenTwoUsersRequest request)
    {
        var result = _messagingService.GetMessagesBetweenTwoUsers(request);
        return ProcessRequestResult(result);
    }

    [HttpPost(Endpoints.Messaging.GET_PREVIEW_MESSAGES)]
    public IActionResult GetPreviewMessages(GetPreviewMessagesRequest request)
    {
        var result = _messagingService.GetPreviewMessages(request);
        return ProcessRequestResult(result);
    }
}