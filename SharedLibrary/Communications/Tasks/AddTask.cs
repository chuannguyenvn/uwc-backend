namespace Commons.Communications.Tasks
{
    public class AddTaskRequest
    {
        public int AssignerAccountId { get; set; }
        public int AssigneeAccountId { get; set; }
        public int McpDataId { get; set; }
    }
}