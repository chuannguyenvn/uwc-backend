using Commons.Models;

namespace Commons.Communications.Tasks
{
    public class GetFocusTaskRequest
    {
        public int WorkerId { get; set; }
    }

    public class GetFocusTaskResponse
    {
        public TaskData? TaskData { get; set; }
    }
}