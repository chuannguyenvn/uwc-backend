using Commons.Communications.Tasks;
using Commons.Endpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Tasks;
using Services.UserProfiles;

namespace Controllers;

// [Authorize]
[ApiController]
[Route("[controller]")]
public class UserProfileController : Controller
{
    private readonly IUserProfileService _userProfileService;

    public UserProfileController(IUserProfileService userProfileService)
    {
        _userProfileService = userProfileService;
    }
    
    
    [HttpGet]
    [Route(Endpoints.UserProfile.GET_USER_PROFILE)]
    public IActionResult GetById([FromRoute] int id)
    {
        var requestResult = _userProfileService.GetUserProfileById(id);
        return ProcessRequestResult(requestResult);
    }
    
    
    [HttpGet]
    [Route(Endpoints.UserProfile.GET_ALL_USER_PROFILES)]
    public IActionResult GetAll()
    {
        var requestResult = _userProfileService.GetAllUserProfiles();
        return ProcessRequestResult(requestResult);
    }
    
    [HttpGet]
    [Route(Endpoints.UserProfile.GET_ALL_DRIVER_PROFILES)]
    public IActionResult GetAllDrivers()
    {
        var requestResult = _userProfileService.GetAllDriverProfiles();
        return ProcessRequestResult(requestResult);
    }
    
    [HttpGet]
    [Route(Endpoints.UserProfile.GET_ALL_CLEANER_PROFILES)]
    public IActionResult GetAllCleaners()
    {
        var requestResult = _userProfileService.GetAllCleanerProfiles();
        return ProcessRequestResult(requestResult);
    }
}