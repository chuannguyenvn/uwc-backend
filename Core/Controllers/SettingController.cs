using Commons.Communications.Settings;
using Commons.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Services.Settings;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class SettingController : Controller
{
    private ISettingService _settingService;

    public SettingController(ISettingService settingService)
    {
        _settingService = settingService;
    }

    [HttpPost(Endpoints.Setting.GET_SETTING)]
    public IActionResult GetSetting(GetSettingRequest request)
    {
        var result = _settingService.GetSetting(request);
        return ProcessRequestResult(result);
    }

    [HttpPost(Endpoints.Setting.UPDATE_SETTING)]
    public IActionResult UpdateSetting(UpdateSettingRequest request)
    {
        var result = _settingService.UpdateSetting(request);
        return ProcessRequestResult(result);
    }

    [HttpPost(Endpoints.Setting.EXPORT_MESSAGES)]
    public IActionResult ExportMessages()
    {
        var result = _settingService.ExportMessages();
        return ProcessRequestResult(result);
    }

    [HttpPost(Endpoints.Setting.CHANGE_PERSONAL_INFORMATION)]
    public IActionResult ChangePersonalInformation(ChangePersonalInformationRequest request)
    {
        var result = _settingService.ChangePersonalInformation(request);
        return ProcessRequestResult(result);
    }

    [HttpPost(Endpoints.Setting.CHANGE_PASSWORD)]
    public IActionResult ChangePassword(ChangePasswordRequest request)
    {
        var result = _settingService.ChangePassword(request);
        return ProcessRequestResult(result);
    }

    [HttpPost(Endpoints.Setting.REPORT_PROBLEM)]
    public IActionResult ReportProblem(ReportProblemRequest request)
    {
        var result = _settingService.ReportProblem(request);
        return ProcessRequestResult(result);
    }
}