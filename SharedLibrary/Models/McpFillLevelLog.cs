using System;

namespace Commons.Models
{
    public class McpFillLevelLog : IndexedEntity
    {
        public int McpDataId { get; set; }
        public McpData McpData { get; set; }
        
        public float McpFillLevel { get; set; }
        public DateTime Timestamp { get; set; }
    }
}