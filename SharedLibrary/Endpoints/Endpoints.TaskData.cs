namespace Commons.Endpoints
{
    public static partial class Endpoints
    {
        public static class TaskData
        {
            private const string TASK_DATA = "taskdata";

            public const string GET_TASKS_OF_WORKER = "get-tasks-of-worker";
            public const string GET_TASKS_WITH_MCP = "get-tasks-with-mcp";
            public const string GET_ALL_TASKS = "get-all-tasks";
            public const string GET_WORKER_PRIORITIZED_TASK = "get-worker-prioritized-task";
            public const string ADD_TASK = "add-task";
            public const string FOCUS_TASK = "focus-task";
            public const string COMPLETE_TASK = "complete-task";
            public const string REJECT_TASK = "reject-task";
            public const string GET_AUTO_TASK_DISTRIBUTION_SETTING = "get-auto-task-distribution-setting";
            public const string TOGGLE_AUTO_TASK_DISTRIBUTION = "toggle-auto-task-distribution";

            public static string GetTasksOfWorker => DOMAIN + "/" + TASK_DATA + "/" + GET_TASKS_OF_WORKER;
            public static string GetTasksWithMcp => DOMAIN + "/" + TASK_DATA + "/" + GET_TASKS_WITH_MCP;
            public static string GetAllTasks => DOMAIN + "/" + TASK_DATA + "/" + GET_ALL_TASKS;
            public static string GetWorkerPrioritizedTask => DOMAIN + "/" + TASK_DATA + "/" + GET_WORKER_PRIORITIZED_TASK;
            public static string AddTask => DOMAIN + "/" + TASK_DATA + "/" + ADD_TASK;
            public static string FocusTask => DOMAIN + "/" + TASK_DATA + "/" + FOCUS_TASK;
            public static string CompleteTask => DOMAIN + "/" + TASK_DATA + "/" + COMPLETE_TASK;
            public static string RejectTask => DOMAIN + "/" + TASK_DATA + "/" + REJECT_TASK;
            public static string GetAutoTaskDistributionSetting => DOMAIN + "/" + TASK_DATA + "/" + GET_AUTO_TASK_DISTRIBUTION_SETTING;
            public static string ToggleAutoTaskDistribution => DOMAIN + "/" + TASK_DATA + "/" + TOGGLE_AUTO_TASK_DISTRIBUTION;
        }
    }
}