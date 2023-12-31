namespace Commons.Communications.Tasks
{
    public class FocusTaskRequest
    {
        public int WorkerId { get; set; }
        public int TaskId { get; set; }
    }
    
    public class FocusTaskBroadcastData
    {
        public int TaskId { get; set; }
        public int WorkerId { get; set; }
    }
}