namespace Requests
{
    public static partial class Endpoints
    {
        public static class TaskData
        {
            private const string TASK_DATA = "taskdata";
            
            public const string ADD_TASK = "add-task";
            public const string COMPLETE_TASK = "complete-task";
            
            public static string AddTask => DOMAIN + "/" + TASK_DATA + "/" + ADD_TASK;
            public static string CompleteTask => DOMAIN + "/" + TASK_DATA + "/" + COMPLETE_TASK;
        }
    }
}