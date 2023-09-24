using Microsoft.AspNetCore.Mvc;
using Services;
using Commons.RequestStatuses;

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

    [HttpGet("focus")]
    public virtual IActionResult Focus()
    {
        return Ok();
    }
}