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

    public ParamRequestResult<string> Login(LoginRequest request)
    {
        if (!_unitOfWork.Accounts.DoesUsernameExist(request.Username)) return new ParamRequestResult<string>(new UsernameNotExist());

        var account = _unitOfWork.Accounts.GetByUsername(request.Username);
        if (account.PasswordHash != request.Password) return new ParamRequestResult<string>(new IncorrectPassword());

        return new ParamRequestResult<string>(new Success(), AuthenticationHelpers.GenerateJwtToken(account, _settings.BearerKey));
    }

    public ParamRequestResult<string> Register(RegisterRequest request)
    {
        if (_unitOfWork.Accounts.DoesUsernameExist(request.Username)) return new ParamRequestResult<string>(new UsernameAlreadyExist());

        var account = new Account
        {
            Username = request.Username,
        };
        account.GenerateSaltAndHash();
        _unitOfWork.Accounts.Add(account);
        _unitOfWork.Complete();

        return new ParamRequestResult<string>(new Success(), AuthenticationHelpers.GenerateJwtToken(account, _settings.BearerKey));
    }
}