using System;
using System.Collections.Generic;
using Commons.Models;

namespace Commons.Communications.Mcps
{
    public class GetFillLevelLogsRequest
    {
        public int McpId { get; set; }
        public int CountLimit { get; set; }
        public DateTime DateTimeLimit { get; set; }
    }

    public class GetFillLevelLogsResponse
    {
        public List<McpFillLevelLog> Results { get; set; }
    }
}