namespace Commons.Endpoints
{
    public static partial class Endpoints
    {
        public static class Setting
        {
            private const string SETTING = "setting";

            public const string GET_SETTING = "getsetting";
            public const string UPDATE_SETTING = "updatesetting";
            public const string CHANGE_PERSONAL_INFORMATION = "changepersonalinformation";
            public const string CHANGE_PASSWORD = "changepassword";
            public const string EXPORT_MESSAGES = "exportmessages";
            public const string REPORT_PROBLEM = "reportproblem";

            public static string GetSetting => DOMAIN + "/" + SETTING + "/" + GET_SETTING;
            public static string UpdateSetting => DOMAIN + "/" + SETTING + "/" + UPDATE_SETTING;
            public static string ChangePersonalInformation => DOMAIN + "/" + SETTING + "/" + CHANGE_PERSONAL_INFORMATION;
            public static string ChangePassword => DOMAIN + "/" + SETTING + "/" + CHANGE_PASSWORD;
            public static string ExportMessages => DOMAIN + "/" + SETTING + "/" + EXPORT_MESSAGES;
            public static string ReportProblem => DOMAIN + "/" + SETTING + "/" + REPORT_PROBLEM;
        }
    }
}