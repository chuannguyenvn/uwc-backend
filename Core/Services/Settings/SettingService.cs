using Commons.Communications.Settings;
using Commons.RequestStatuses;
using Commons.RequestStatuses.Authentication;
using Repositories.Managers;
using Services.Authentication;

namespace Services.Settings;

public class SettingService : ISettingService
{
    private readonly IUnitOfWork _unitOfWork;

    public SettingService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public ParamRequestResult<GetSettingResponse> GetSetting(GetSettingRequest request)
    {
        if (!_unitOfWork.AccountRepository.DoesIdExist(request.AccountId)) return new ParamRequestResult<GetSettingResponse>(new DataEntryNotFound());

        var setting = _unitOfWork.SettingRepository.GetById(request.AccountId);

        return new ParamRequestResult<GetSettingResponse>(new Success(), new GetSettingResponse() { Setting = setting });
    }

    public RequestResult UpdateSetting(UpdateSettingRequest request)
    {
        _unitOfWork.SettingRepository.Update(request.NewSetting);
        _unitOfWork.Complete();

        return new RequestResult(new Success());
    }

    public ParamRequestResult<ExportMessagesResponse> ExportMessages()
    {
        // TODO: Implement
        return new ParamRequestResult<ExportMessagesResponse>(new Success(), new ExportMessagesResponse());
    }

    public RequestResult ChangePersonalInformation(ChangePersonalInformationRequest request)
    {
        if (!_unitOfWork.AccountRepository.DoesIdExist(request.AccountId)) return new RequestResult(new DataEntryNotFound());

        var profile = _unitOfWork.UserProfileRepository.GetById(request.AccountId);

        if (request.FirstName != null) profile.FirstName = request.FirstName;
        if (request.LastName != null) profile.LastName = request.LastName;
        if (request.LastName != null) profile.LastName = request.LastName;
        if (request.Gender != null) profile.Gender = request.Gender.Value;
        if (request.DateOfBirth != null) profile.DateOfBirth = request.DateOfBirth.Value;
        if (request.Address != null) profile.Address = request.Address;

        _unitOfWork.Complete();

        return new RequestResult(new Success());
    }

    public ParamRequestResult<ChangePasswordResponse> ChangePassword(ChangePasswordRequest request)
    {
        if (!_unitOfWork.AccountRepository.DoesIdExist(request.AccountId))
            return new ParamRequestResult<ChangePasswordResponse>(new DataEntryNotFound());

        var account = _unitOfWork.AccountRepository.GetById(request.AccountId);

        var oldHash = AuthenticationHelpers.ComputeHash(request.OldPassword, account.PasswordSalt);
        if (oldHash != account.PasswordHash) return new ParamRequestResult<ChangePasswordResponse>(new IncorrectPassword());

        var newHash = AuthenticationHelpers.ComputeHash(request.NewPassword, account.PasswordSalt);
        account.PasswordHash = newHash;
        _unitOfWork.AccountRepository.Update(account);
        _unitOfWork.Complete();

        return new ParamRequestResult<ChangePasswordResponse>(new Success(), new ChangePasswordResponse() { Success = true });
    }

    public RequestResult ReportProblem(ReportProblemRequest request)
    {
        // TODO: Implement
        return new ParamRequestResult<ExportMessagesResponse>(new Success(), new ExportMessagesResponse());
    }
}