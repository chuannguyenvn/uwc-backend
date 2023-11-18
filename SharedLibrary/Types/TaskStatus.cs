using System;

namespace Commons.Types
{
    public enum TaskStatus
    {
        NotStarted,
        InProgress,
        Completed,
        Rejected,
    }

    public static class TaskStatusHelper
    {
        public static string GetFriendlyString(this TaskStatus status)
        {
            switch (status)
            {
                case TaskStatus.NotStarted:
                    return "Pending";
                case TaskStatus.InProgress:
                    return "Ongoing";
                case TaskStatus.Completed:
                    return "Completed";
                case TaskStatus.Rejected:
                    return "Rejected";
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }
    }
}