using Microsoft.AspNetCore.Mvc;
using RequestStatuses;
using Services;

namespace Controllers;

public class Controller : ControllerBase
{
    protected IActionResult ProcessRequestResult(RequestResult requestResult)
    {
        switch (requestResult.RequestStatus.StatusType)
        {
            case HttpResponseStatusType.Ok:
                return Ok(requestResult.Payload);
            case HttpResponseStatusType.BadRequest:
                return BadRequest(requestResult.RequestStatus);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}