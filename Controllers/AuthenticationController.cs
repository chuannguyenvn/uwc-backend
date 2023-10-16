using Microsoft.AspNetCore.Mvc;
using Services.Authentication;
using Commons.Communications.Authentication;
using Requests;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : Controller
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IFacialRecognitionService _facialRecognitionService;

    public AuthenticationController(IAuthenticationService authenticationService, IFacialRecognitionService facialRecognitionService)
    {
        _authenticationService = authenticationService;
        _facialRecognitionService = facialRecognitionService;
    }

    [HttpPost(Endpoints.Authentication.LOGIN)]
    public IActionResult Login(LoginRequest request)
    {
        var result = _authenticationService.Login(request);
        return ProcessRequestResult(result);
    }

    [HttpPost(Endpoints.Authentication.REGISTER)]
    public IActionResult Register(RegisterRequest request)
    {
        var result = _authenticationService.Register(request);
        return ProcessRequestResult(result);
    }
    
    [HttpPost(Endpoints.Authentication.LOGIN_WITH_FACE)]
    public IActionResult LoginWithFace(LoginWithFaceRequest request)
    {
        var result = _facialRecognitionService.LoginWithFace(request);
        return ProcessRequestResult(result);
    }
    
    [HttpPost(Endpoints.Authentication.REGISTER_FACE)]
    public IActionResult RegisterFace(RegisterFaceRequest request)
    {
        var result = _facialRecognitionService.RegisterFace(request);
        return ProcessRequestResult(result);
    }
    
    [HttpPost(Endpoints.Authentication.DELETE_FACE)]
    public IActionResult DeleteFace(DeleteFaceRequest request)
    {
        var result = _facialRecognitionService.DeleteFace(request);
        return ProcessRequestResult(result);
    }
}