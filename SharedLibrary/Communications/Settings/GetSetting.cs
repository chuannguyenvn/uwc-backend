using Commons.Models;

namespace Commons.Communications.Settings
{
    public class GetSettingRequest
    {
        public int AccountId { get; set; }
    }

    public class GetSettingResponse
    {
        public Setting Setting { get; set; }
    }
}