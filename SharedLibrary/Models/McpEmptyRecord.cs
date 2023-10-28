using System;

namespace Commons.Models
{
    public class McpEmptyRecord : IndexedEntity
    {
        public int McpDataId { get; set; }
        public McpData McpData { get; set; }

        public int EmptyingWorkerId { get; set; }
        public UserProfile EmptyingWorker { get; set; }
        public DateTime Timestamp { get; set; }
        public float McpFillLevelBeforeEmptying { get; set; }
        public float McpFillLevelAfterEmptying { get; set; }
    }
}