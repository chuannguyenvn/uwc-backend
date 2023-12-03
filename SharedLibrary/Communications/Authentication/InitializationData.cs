﻿using Commons.Communications.Map;
using Commons.Communications.Mcps;
using Commons.Models;
using SharedLibrary.Communications.OnlineStatus;

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