using Commons.Communications.Authentication;

namespace Services.Authentication;

public interface IAuthenticationService
{
    public ParamRequestResult<LoginResponse> Login(LoginRequest request);
    public ParamRequestResult<RegisterResponse> Register(RegisterRequest request);
    public ParamRequestResult<LoginResponse> LoginWithFace(LoginWithFaceRequest request);
    public ParamRequestResult<RegisterFaceResponse> RegisterFace(RegisterFaceRequest request);
}