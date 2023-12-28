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

        /// <summary>
        /// Lower number means higher priority
        /// </summary>
        public int Priority { get; set; }
        public int? GroupId { get; set; }

        public DateTime CreatedTimestamp { get; set; } = DateTime.UtcNow;
        public DateTime CompleteByTimestamp { get; set; }
        public DateTime LastStatusChangeTimestamp { get; set; } = DateTime.UtcNow;
        public TaskStatus TaskStatus { get; set; } = TaskStatus.NotStarted;
    }
}