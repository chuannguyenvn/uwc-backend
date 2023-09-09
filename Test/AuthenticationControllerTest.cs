using Commons.Communications.Authentication;
using Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Services;
using Services.Authentication;

namespace Test;

public class AuthenticationControllerTest
{
    private AuthenticationController _authenticationController;
    private Mock<IAuthenticationService> _authenticationServiceMock;

    private Mock<RegisterRequest> _registerRequestMock;
    private Mock<LoginRequest> _loginRequestMock;

    [SetUp]
    public void Setup()
    {
        _authenticationServiceMock = new Mock<IAuthenticationService>();
        _authenticationController = new AuthenticationController(_authenticationServiceMock.Object);

        _registerRequestMock = new Mock<RegisterRequest>();
        _loginRequestMock = new Mock<LoginRequest>();

        _registerRequestMock.Setup(request => request.Username).Returns("test_username");
        _registerRequestMock.Setup(request => request.Password).Returns("test_password");

        _loginRequestMock.Setup(request => request.Username).Returns("test_username");
        _loginRequestMock.Setup(request => request.Password).Returns("test_password");
    }

    [Test]
    public void Login()
    {
        var authenticationService = new Mock<IAuthenticationService>();

        var loginRequest = new LoginRequest
        {
            Username = "test",
            Password = "test"
        };
        var expectedResult = new RequestResult(true, "", null);

        authenticationService
            .Setup(service => service.Login(loginRequest))
            .Returns(expectedResult);
    }

    [Test]
    public void Register()
    {
        _authenticationServiceMock.Setup(service => service.Register(_registerRequestMock.Object)).Returns(new RequestResult(true, "", null));

        var result = _authenticationController.Register(_registerRequestMock.Object);

        Assert.IsInstanceOf<OkResult>(result);
    }
}