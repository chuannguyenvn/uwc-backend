using Commons.Communications.Authentication;
using Microsoft.AspNetCore.Mvc;
using Services.Authentication;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        var result = _authenticationService.Login(request);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok();
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
    {
        var result = _authenticationService.Register(request);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok();
    }
}