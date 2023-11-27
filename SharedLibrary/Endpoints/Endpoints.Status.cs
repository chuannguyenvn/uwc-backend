namespace Commons.Endpoints
{
    public static partial class Endpoints
    {
        public static class Status
        {
            private const string STATUS = "status";

            public const string GET_WORKING_STATUS = "get-working-status";

            public static string GetWorkingStatus => DOMAIN + "/" + STATUS + "/" + GET_WORKING_STATUS;
        }
    }
}