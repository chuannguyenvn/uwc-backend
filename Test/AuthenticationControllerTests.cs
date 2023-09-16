using Commons.Communications.Authentication;
using Repositories;
using RequestStatuses;
using RequestStatuses.Authentication;
using Services.Authentication;

namespace Test;

public class AuthenticationControllerTests
{
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
    
    [Test]
    public void RegisterThenLogin()
    {
        var mockUnitOfWork = new MockUnitOfWork();
        var authenticationService = new AuthenticationService(mockUnitOfWork);

        var registerResult = authenticationService.Register(new RegisterRequest()
        {
            Username = "new_user",
            Password = "new_password",
        });

        Assert.IsInstanceOf<Success>(registerResult.RequestStatus);
        
        var loginResult = authenticationService.Login(new LoginRequest()
        {
            Username = "new_user",
            Password = "new_password",
        });
        
        Assert.IsInstanceOf<Success>(loginResult.RequestStatus);
    }

    [Test]
    public void DuplicatedRegister()
    {
        var mockUnitOfWork = new MockUnitOfWork();
        var authenticationService = new AuthenticationService(mockUnitOfWork);

        var firstRegisterResult = authenticationService.Register(new RegisterRequest()
        {
            Username = "new_user",
            Password = "new_password",
        });

        Assert.IsInstanceOf<Success>(firstRegisterResult.RequestStatus);
        
        var secondRegisterResult = authenticationService.Register(new RegisterRequest()
        {
            Username = "new_user",
            Password = "new_password",
        });
        
        Assert.IsInstanceOf<UsernameAlreadyExist>(secondRegisterResult.RequestStatus);
    }
}