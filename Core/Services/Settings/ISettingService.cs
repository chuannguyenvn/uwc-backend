using Commons.Communications.Settings;

namespace Services.Settings;

public interface ISettingService
{
    public ParamRequestResult<GetSettingResponse> GetSetting(GetSettingRequest request);
    public RequestResult UpdateSetting(UpdateSettingRequest request);
    public ParamRequestResult<ExportMessagesResponse> ExportMessages();
    public RequestResult ChangePersonalInformation(ChangePersonalInformationRequest request);
    public ParamRequestResult<ChangePasswordResponse> ChangePassword(ChangePasswordRequest request);
    public RequestResult ReportProblem(ReportProblemRequest request);
}