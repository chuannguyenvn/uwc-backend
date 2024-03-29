using Commons.Categories;
using Repositories.Managers;
using Services.Authentication;
using Commons.Communications.Authentication;
using Commons.RequestStatuses;
using Commons.RequestStatuses.Authentication;
using Commons.Types;

namespace Test;

public class AuthenticationServiceTests
{
    private Settings _mockSettings;

    [SetUp]
    public void SetUp()
    {
        _mockSettings = new Settings()
        {
            BearerKey = "mock_key_mock_key_mock_key_mock_key",
        };
    }

    [Test]
    public void CorrectLogin()
    {
        var mockUnitOfWork = new MockUnitOfWork();
        var authenticationService = new AuthenticationService(mockUnitOfWork, _mockSettings);

        var result = authenticationService.Login(new LoginRequest()
        {
            Username = "supervisor_supervisor",
            Password = "password",
            IsFromDesktop = true,
        });

        Assert.IsInstanceOf<Success>(result.RequestStatus);
    }

    [Test]
    public void WrongUsernameLogin()
    {
        var mockUnitOfWork = new MockUnitOfWork();
        var authenticationService = new AuthenticationService(mockUnitOfWork, _mockSettings);

        var result = authenticationService.Login(new LoginRequest()
        {
            Username = "weird_username",
            Password = "password",
            IsFromDesktop = true,
        });

        Assert.IsInstanceOf<UsernameNotExist>(result.RequestStatus);
    }

    [Test]
    public void WrongPasswordLogin()
    {
        var mockUnitOfWork = new MockUnitOfWork();
        var authenticationService = new AuthenticationService(mockUnitOfWork, _mockSettings);

        var result = authenticationService.Login(new LoginRequest()
        {
            Username = "supervisor_supervisor",
            Password = "wrong_password",
            IsFromDesktop = true,
        });

        Assert.IsInstanceOf<IncorrectPassword>(result.RequestStatus);
    }

    [Test]
    public void RegisterThenLogin()
    {
        var mockUnitOfWork = new MockUnitOfWork();
        var authenticationService = new AuthenticationService(mockUnitOfWork, _mockSettings);

        var registerResult = authenticationService.Register(new RegisterRequest
        {
            Username = "new_user",
            Password = "new_password",
            UserRole = UserRole.Supervisor,
            FirstName = "test",
            LastName = "test",
            Gender = Gender.Male,
            DateOfBirth = default,
            Address = "test",

        });

        Assert.IsInstanceOf<Success>(registerResult.RequestStatus);

        var loginResult = authenticationService.Login(new LoginRequest
        {
            Username = "new_user",
            Password = "new_password",
            IsFromDesktop = true,

        });

        Assert.IsInstanceOf<Success>(loginResult.RequestStatus);
    }

    [Test]
    public void DuplicatedRegister()
    {
        var mockUnitOfWork = new MockUnitOfWork();
        var authenticationService = new AuthenticationService(mockUnitOfWork, _mockSettings);

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