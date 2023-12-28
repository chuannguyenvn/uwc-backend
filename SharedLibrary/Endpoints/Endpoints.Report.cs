namespace Commons.Endpoints
{
    public static partial class Endpoints
    {
        public static class Report
        {
            private const string REPORT = "report";

            public const string GET = "get";
            public const string GET_FILE = "get-file";

            public static string Get => DOMAIN + "/" + REPORT + "/" + GET;
            public static string GetFile => DOMAIN + "/" + REPORT + "/" + GET_FILE;
        }
    }
}