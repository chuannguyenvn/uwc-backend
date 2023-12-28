using System;
using System.Collections.Generic;

namespace Commons.Communications.Reports
{
    public class GetReportFileRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class GetReportFileResponse
    {
        public List<DateTime> McpFillLevelTimestamps { get; set; }
        public List<int> FillLevelMcpIds { get; set; }
        public List<string> FillLevelMcpAddresses { get; set; }
        public List<float> McpFillLevelValues { get; set; }

        public List<DateTime> McpEmptiedTimestamps { get; set; }
        public List<int> EmptyingMcpIds { get; set; }
        public List<string> EmptyingMcpAddresses { get; set; }
        public List<int> WorkerIds { get; set; }
        public List<string> WorkerNames { get; set; }
    }
}