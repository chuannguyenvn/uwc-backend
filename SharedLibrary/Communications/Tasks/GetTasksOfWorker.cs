using System.Collections.Generic;
using Commons.Models;

namespace Commons.Communications.Tasks
{
    public class GetTasksOfWorkerRequest
    {
        public int WorkerId { get; set; }
    }

    public class GetTasksOfWorkerResponse
    {
        public List<TaskData> Tasks { get; set; }
    }
}