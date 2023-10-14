using Commons.Categories;
using Repositories.Managers;
using Commons.Communications.Authentication;
using Commons.Models;
using Commons.RequestStatuses;
using Commons.RequestStatuses.Authentication;

namespace Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly Settings _settings;

    public AuthenticationService(IUnitOfWork unitOfWork, Settings settings)
    {
        _unitOfWork = unitOfWork;
        _settings = settings;
    }

    public ParamRequestResult<LoginResponse> Login(LoginRequest request)
    {
        if (!_unitOfWork.Accounts.DoesUsernameExist(request.Username)) return new ParamRequestResult<LoginResponse>(new UsernameNotExist());

        var account = _unitOfWork.Accounts.GetByUsername(request.Username);
        if (account.PasswordHash != AuthenticationHelpers.ComputeHash(request.Password, account.PasswordSalt))
            return new ParamRequestResult<LoginResponse>(new IncorrectPassword());

        var userRole = _unitOfWork.UserProfiles.GetById(account.UserProfileID).UserRole;
        if (request.IsFromDesktop && userRole != UserRole.Supervisor)
            return new ParamRequestResult<LoginResponse>(new InvalidRoleWhenLogin());
        if (!request.IsFromDesktop && userRole == UserRole.Supervisor)
            return new ParamRequestResult<LoginResponse>(new InvalidRoleWhenLogin());
        
        var loginResponse = new LoginResponse
        {
            JwtToken = AuthenticationHelpers.GenerateJwtToken(account, _settings.BearerKey),
            UserId = account.UserProfileID,
        };

        return new ParamRequestResult<LoginResponse>(new Success(), loginResponse);
    }

    public ParamRequestResult<RegisterResponse> Register(RegisterRequest request)
    {
        if (_unitOfWork.Accounts.DoesUsernameExist(request.Username)) return new ParamRequestResult<RegisterResponse>(new UsernameAlreadyExist());

        var account = new Account
        {
            Username = request.Username,
            PasswordHash = request.Password,
        };
        account.GenerateSaltAndHash();
        _unitOfWork.Accounts.Add(account);
        _unitOfWork.Complete();

        var registerResponse = new RegisterResponse
        {
            JwtToken = AuthenticationHelpers.GenerateJwtToken(account, _settings.BearerKey),
            UserId = account.UserProfileID,
        };

        return new ParamRequestResult<RegisterResponse>(new Success(), registerResponse);
    }
}