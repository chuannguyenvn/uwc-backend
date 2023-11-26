using System;
using Commons.Types;

namespace Commons.Models
{
    public class TaskData : IndexedEntity
    {
        public int AssignerId { get; set; }
        public UserProfile AssignerProfile { get; set; }

        public int? AssigneeId { get; set; }
        public UserProfile? AssigneeProfile { get; set; }

        public int McpDataId { get; set; }
        public McpData McpData { get; set; }

        public DateTime CreatedTimestamp { get; set; } = DateTime.Now;
        public DateTime CompleteByTimestamp { get; set; }
        public DateTime LastStatusChangeTimestamp { get; set; } = DateTime.Now;
        public TaskStatus TaskStatus { get; set; } = TaskStatus.NotStarted;
    }
}