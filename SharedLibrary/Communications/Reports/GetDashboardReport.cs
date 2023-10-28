using System;
using System.Collections.Generic;
using Commons.Types;

namespace Commons.Communications.Reports
{
    public class GetDashboardReportResponse
    {
        public int TotalTasksCompleted { get; set; }
        public int TotalTasksCreated { get; set; }
        public float AverageTaskCompletionTimeInMinutes { get; set; }
        
        public int OnlineWorkers { get; set; }
        public int TotalWorkers { get; set; }
        
        public float CurrentTemperature { get; set; }
        public ChanceOfPrecipitation ChanceOfPrecipitation { get; set; }
        
        public List<DateTime> TotalMcpFillLevelTimestamps { get; set; }
        public List<float> TotalMcpFillLevelValues { get; set; }
        
        public List<DateTime> McpEmptiedTimestamps { get; set; }
    }
}