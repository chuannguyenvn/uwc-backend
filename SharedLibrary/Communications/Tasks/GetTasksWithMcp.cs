using System.Collections.Generic;
using Commons.Models;

namespace Commons.Communications.Tasks
{
    public class GetTasksWithMcpRequest
    {
        public int McpId { get; set; }
    }

    public class GetTasksWithMcpResponse
    {
        public List<TaskData> Tasks { get; set; }
    }
}