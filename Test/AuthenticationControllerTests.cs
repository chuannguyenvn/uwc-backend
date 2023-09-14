using Commons.Communications.Authentication;
using Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Repositories;
using Repositories.Implementations.Accounts;
using RequestStatuses;
using RequestStatuses.Authentication;
using Services;
using Services.Authentication;

namespace Test;

public class AuthenticationControllerTests
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
    public void CorrectLogin()
    {
        var mockUnitOfWork = new MockUnitOfWork();
        var authenticationService = new AuthenticationService(mockUnitOfWork);

        var result = authenticationService.Login(new LoginRequest()
        {
            Username = "admin",
            Password = "password",
        });

        Assert.IsInstanceOf<Success>(result.RequestStatus);
    }

    [Test]
    public void WrongUsernameLogin()
    {
        var mockUnitOfWork = new MockUnitOfWork();
        var authenticationService = new AuthenticationService(mockUnitOfWork);

        var result = authenticationService.Login(new LoginRequest()
        {
            Username = "weird_username",
            Password = "password",
        });

        Assert.IsInstanceOf<UsernameNotExist>(result.RequestStatus);
    }

    [Test]
    public void WrongPasswordLogin()
    {
        var mockUnitOfWork = new MockUnitOfWork();
        var authenticationService = new AuthenticationService(mockUnitOfWork);

        var result = authenticationService.Login(new LoginRequest()
        {
            Username = "admin",
            Password = "wrong_password",
        });

        Assert.IsInstanceOf<IncorrectPassword>(result.RequestStatus);
    }
}