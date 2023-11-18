using System;
using Commons.Types;

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
        public DateTime LastStatusChangeTimestamp { get; set; }
        public TaskStatus TaskStatus { get; set; }
    }
}