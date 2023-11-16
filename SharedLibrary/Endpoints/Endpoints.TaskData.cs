namespace Commons.Endpoints
{
    public static partial class Endpoints
    {
        public static class TaskData
        {
            private const string TASK_DATA = "taskdata";

            public const string GET_TASKS_OF_WORKER = "get-tasks-of-worker";
            public const string GET_ALL_TASKS = "get-all-tasks";
            public const string ADD_TASK = "add-task";
            public const string COMPLETE_TASK = "complete-task";
            public const string REJECT_TASK = "reject-task";

            public static string GetTasksOfWorker => DOMAIN + "/" + TASK_DATA + "/" + GET_TASKS_OF_WORKER;
            public static string GetAllTasks => DOMAIN + "/" + TASK_DATA + "/" + GET_ALL_TASKS;
            public static string AddTask => DOMAIN + "/" + TASK_DATA + "/" + ADD_TASK;
            public static string CompleteTask => DOMAIN + "/" + TASK_DATA + "/" + COMPLETE_TASK;
            public static string RejectTask => DOMAIN + "/" + TASK_DATA + "/" + REJECT_TASK;
        }
    }
}