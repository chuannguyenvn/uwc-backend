using Commons.Communications.Map;
using Commons.Communications.Mcps;
using Commons.Communications.Status;
using Commons.Models;
using Commons.Types;

namespace Commons.Communications.Authentication
{
    public class InitializationData
    {
        public McpLocationBroadcastData McpLocationBroadcastData { get; set; }
        public McpFillLevelBroadcastData McpFillLevelBroadcastData { get; set; }
        public OnlineStatusBroadcastData OnlineStatusBroadcastData { get; set; }
        public Setting Setting { get; set; }
    }
}