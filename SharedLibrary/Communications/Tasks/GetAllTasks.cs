using System.Collections.Generic;
using Commons.Models;

namespace Commons.Communications.Tasks
{
    public class GetAllTasksResponse
    {
        public List<TaskData> Tasks { get; set; }
    }
}