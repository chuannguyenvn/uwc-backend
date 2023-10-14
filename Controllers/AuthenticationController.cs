﻿using Microsoft.AspNetCore.Mvc;
using Services.Authentication;
using Commons.Communications.Authentication;
using Requests;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : Controller
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
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
}