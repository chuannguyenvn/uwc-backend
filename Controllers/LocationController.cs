using System.Security.Claims;
using Commons.Communications.Location;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Location;

namespace Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class LocationController : Controller
{
    private readonly ILocationService _locationService;

    public LocationController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    [HttpPost]
    [Route("location/update")]
    public RequestResult UpdateLocation(LocationUpdateRequest request)
    {
        return _locationService.UpdateLocation(int.Parse(User.FindFirstValue("id")), request);
    }
}