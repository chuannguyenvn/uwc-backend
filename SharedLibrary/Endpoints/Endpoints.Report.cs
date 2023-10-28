namespace Commons.Endpoints
{
    public static partial class Endpoints
    {
        public static class Report
        {
            private const string REPORT = "report";

            public const string GET = "get";

            public static string Get => DOMAIN + "/" + REPORT + "/" + GET;
        }
    }
}