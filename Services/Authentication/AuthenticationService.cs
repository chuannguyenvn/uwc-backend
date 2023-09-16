using Repositories.Managers;
using Commons.Communications.Authentication;
using Commons.Models;
using Commons.RequestStatuses;
using Commons.RequestStatuses.Authentication;

namespace Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUnitOfWork _unitOfWork;

    public AuthenticationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public RequestResult Login(LoginRequest request)
    {
        if (!_unitOfWork.Accounts.DoesUsernameExist(request.Username)) return new RequestResult(new UsernameNotExist());

        var account = _unitOfWork.Accounts.GetByUsername(request.Username);
        if (account.PasswordHash != request.Password) return new RequestResult(new IncorrectPassword());

        return new RequestResult(new Success());
    }

    public RequestResult Register(RegisterRequest request)
    {
        if (_unitOfWork.Accounts.DoesUsernameExist(request.Username)) return new RequestResult(new UsernameAlreadyExist());

        var account = new Account
        {
            Username = request.Username,
            PasswordHash = request.Password,
            PasswordSalt = "TODO",
        };
        _unitOfWork.Accounts.Add(account);
        _unitOfWork.Complete();

        return new RequestResult(new Success());
    }
}