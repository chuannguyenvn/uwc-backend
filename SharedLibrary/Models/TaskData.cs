using System;

namespace Commons.Models
{
    public class TaskData : IndexedEntity
    {
        public int AssignerAccountId { get; set; }
        public Account AssignerAccount { get; set; }

        public int AssigneeAccountId { get; set; }
        public Account AssigneeAccount { get; set; }

        public int McpDataId { get; set; }
        public McpData McpData { get; set; }
        

        public DateTime CreatedTimestamp { get; set; }
        public DateTime CompleteByTimestamp { get; set; }
        public DateTime? CompletedTimestamp { get; set; }
        public bool IsCompleted { get; set; }
    }
}