using Commons.Communications.Settings;

namespace Services.Settings;

public interface ISettingService
{
    public RequestResult UpdateSetting(UpdateSettingRequest request);
}