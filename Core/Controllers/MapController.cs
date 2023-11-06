using Commons.Communications.Map;
using Commons.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Services.Map;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class MapController : Controller
{
    private readonly ILocationService _locationService;
    private readonly IDirectionService _directionService;

    public MapController(ILocationService locationService, IDirectionService directionService)
    {
        _locationService = locationService;
        _directionService = directionService;
    }

    [HttpPost(Endpoints.Map.GET_LOCATION)]
    public IActionResult GetLocation(GetLocationRequest request)
    {
        var result = _locationService.GetLocation(request);
        return ProcessRequestResult(result);
    }

    [HttpPost(Endpoints.Map.UPDATE_LOCATION)]
    public IActionResult UpdateLocation(LocationUpdateRequest request)
    {
        var result = _locationService.UpdateLocation(request);
        return ProcessRequestResult(result);
    }
    
    [HttpPost(Endpoints.Map.GET_DIRECTION)]
    public IActionResult GetDirection(GetDirectionRequest request)
    {
        var result = _directionService.GetDirection(request);
        return ProcessRequestResult(result);
    }
}