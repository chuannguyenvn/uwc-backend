using System;

namespace Commons.Models
{
    public class McpFillLevelLog : IndexedEntity
    {
        public int McpDataId { get; set; }
        public McpData McpData { get; set; }
        
        /// <summary>
        /// Fill level is a float between 0 and 1.
        /// </summary>
        public float McpFillLevel { get; set; }
        public DateTime Timestamp { get; set; }
    }
}