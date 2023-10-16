using Commons.Communications.Authentication;

namespace Services.Authentication;

public interface IFacialRecognitionService
{
    public ParamRequestResult<LoginWithFaceResponse> LoginWithFace(LoginWithFaceRequest request);
    public ParamRequestResult<RegisterFaceResponse> RegisterFace(RegisterFaceRequest request);
    public RequestResult DeleteFace(DeleteFaceRequest request);
}