using Commons.Models;

namespace Commons.Communications.Tasks
{
    public class GetWorkerPrioritizedTaskRequest
    {
        public int WorkerId { get; set; }
    }

    public class GetWorkerPrioritizedTaskResponse
    {
        public TaskData? Task { get; set; }
    }
}