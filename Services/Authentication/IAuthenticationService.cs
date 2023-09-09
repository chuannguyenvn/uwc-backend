using Commons.Communications.Authentication;

namespace Services.Authentication;

public interface IAuthenticationService
{
    public RequestResult Login(LoginRequest request);
    public RequestResult Register(RegisterRequest request);
}