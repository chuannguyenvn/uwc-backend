using System;
using Commons.Models;

namespace Commons.Communications.Mcps
{
    public class GetSingleMcpDataRequest
    {
        public int McpId { get; set; }
        public int HistoryCountLimit { get; set; }
        public DateTime HistoryDateTimeLimit { get; set; }
    }

    public class GetSingleMcpDataResponse
    {
        public McpData Result { get; set; }
    }
}