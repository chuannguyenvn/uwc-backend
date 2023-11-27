using Commons.Models;

namespace Commons.Communications.Status
{
    public class GetWorkingStatusRequest
    {
        public int WorkerId { get; set; }
    }

    public class GetWorkingStatusResponse
    {
        public bool IsOnline { get; set; }
        public UserProfile UserProfile { get; set; }
        public TaskData? FocusedTask { get; set; }
    }
}