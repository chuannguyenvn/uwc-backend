using Commons.Models;

namespace Commons.Communications.Settings
{
    public class UpdateSettingRequest
    {
        public int AccountId { get; set; }
        public Setting NewSetting { get; set; }
    }
}