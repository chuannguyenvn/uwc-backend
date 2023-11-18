using System;
using System.Collections.Generic;
using Commons.Models;

namespace Commons.Communications.Mcps
{
    public class GetEmptyRecordsRequest
    {
        public int McpId { get; set; }
        public int CountLimit { get; set; }
        public DateTime DateTimeLimit { get; set; }
    }

    public class GetEmptyRecordsResponse
    {
        public List<McpEmptyRecord> Results { get; set; }
    }
}