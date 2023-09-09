using Commons.Communications.Authentication;
using Commons.Models;
using Repositories;

namespace Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly UnitOfWork _unitOfWork;

    public AuthenticationService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public RequestResult Login(LoginRequest request)
    {
        if (!_unitOfWork.Accounts.DoesUsernameExist(request.Username)) return new RequestResult(false, "Username does not exist", null);

        var account = _unitOfWork.Accounts.GetByUsername(request.Username);
        if (account.PasswordHash != request.Password) return new RequestResult(false, "Password is incorrect", null);

        return new RequestResult(true, "", null);
    }

    public RequestResult Register(RegisterRequest request)
    {
        if (_unitOfWork.Accounts.DoesUsernameExist(request.Username)) return new RequestResult(false, "Username already exists", null);

        var account = new Account
        {
            Username = request.Username,
            PasswordHash = request.Password
        };
        _unitOfWork.Accounts.Add(account);
        _unitOfWork.Complete();

        return new RequestResult(true, "", null);
    }
}