using Commons.Communications.Authentication;
using Commons.Models;
using Commons.RequestStatuses;
using Commons.RequestStatuses.Authentication;
using FaceRecognitionDotNet;
using Repositories.Managers;
using TorchSharp;

namespace Services.Authentication;

public class FacialRecognitionService : IFacialRecognitionService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly Settings _settings;

    public FacialRecognitionService(UnitOfWork unitOfWork, Settings settings)
    {
        _unitOfWork = unitOfWork;
        _settings = settings;
    }

    public ParamRequestResult<LoginWithFaceResponse> LoginWithFace(LoginWithFaceRequest request)
    {
        // Viết logic nhận diện khuôn mặt ở đây
        var isValid = false; // Nếu nhận diện thành công thì isValid = true
        torch.device(torch.cuda_is_available() ? DeviceType.CUDA : DeviceType.CPU);


        // ...


        // Kết thúc logic nhận diện khuôn mặt
        // Nếu nhận diện thành công -> trả về Credentials
        // Nếu nhận diện thất bại -> báo lỗi
        if (isValid)
        {
            var account = _unitOfWork.Accounts.GetById(request.AccountId);
            return new ParamRequestResult<LoginWithFaceResponse>(new Success(), new LoginWithFaceResponse
            {
                IsValid = false,
                Credentials = new Credentials
                {
                    JwtToken = AuthenticationHelpers.GenerateJwtToken(account, _settings.BearerKey),
                    AccountId = request.AccountId
                }
            });
        }
        else
        {
            return new ParamRequestResult<LoginWithFaceResponse>(new CannotRecognizeFace());
        }
    }

    public ParamRequestResult<RegisterFaceResponse> RegisterFace(RegisterFaceRequest request)
    {
        // Viết logic đăng ký khuôn mặt ở đây
        var isSuccessful = false; // Nếu đăng ký thành công thì isSuccessful = true


        // ...


        // Kết thúc logic đăng ký khuôn mặt
        // Nếu đăng ký thành công -> trả về isSuccessful = true
        // Nếu đăng ký thất bại -> báo lỗi
        if (isSuccessful)
        {
            var account = _unitOfWork.Accounts.GetById(request.AccountId);
            account.UseFaceRecognition = true;
            account.FaceVector = new List<float>(); // Thay bằng vector gì đó của m
            _unitOfWork.Complete();

            return new ParamRequestResult<RegisterFaceResponse>(new Success(), new RegisterFaceResponse
            {
                IsSuccessful = true
            });
        }
        else
        {
            return new ParamRequestResult<RegisterFaceResponse>(new CannotRecognizeFace(), new RegisterFaceResponse
            {
                IsSuccessful = false
            });
        }
    }

    public RequestResult DeleteFace(DeleteFaceRequest request)
    {
        var account = _unitOfWork.Accounts.GetById(request.AccountId);
        if (account.UseFaceRecognition)
        {
            account.UseFaceRecognition = false;
            account.FaceVector = null;
        }

        _unitOfWork.Complete();

        return new RequestResult(new Success());
    }
}