using Commons.Communications.Authentication;

namespace Services.Authentication;

public interface IAuthenticationService
{
    public ParamRequestResult<string> Login(LoginRequest request);
    public ParamRequestResult<string> Register(RegisterRequest request);
}