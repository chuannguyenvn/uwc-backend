using Commons.Communications.Authentication;
using Repositories.Managers;

namespace Services.Authentication;

public class FacialRecognitionService : IFacialRecognitionService
{
    private readonly UnitOfWork _unitOfWork;

    public FacialRecognitionService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public ParamRequestResult<LoginWithFaceResponse> LoginWithFace(LoginWithFaceRequest request)
    {
        throw new NotImplementedException();
    }

    public RequestResult RegisterFace(RegisterFaceRequest request)
    {
        throw new NotImplementedException();
    }

    public RequestResult DeleteFace(DeleteFaceRequest request)
    {
        throw new NotImplementedException();
    }
}