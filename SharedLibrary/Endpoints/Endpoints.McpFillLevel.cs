namespace Commons.Endpoints
{
    public static partial class Endpoints
    {
        public static class McpFillLevel
        {
            private const string MCP_FILL_LEVEL = "mcpfilllevel";

            public const string GET = "get";
            public const string GET_ALL = "getall";
            public const string SET = "set";
            public const string EMPTY = "empty";

            public static string Get => DOMAIN + "/" + MCP_FILL_LEVEL + "/" + GET;
            public static string GetAll => DOMAIN + "/" + MCP_FILL_LEVEL + "/" + GET_ALL;
            public static string Set => DOMAIN + "/" + MCP_FILL_LEVEL + "/" + SET;
            public static string Empty => DOMAIN + "/" + MCP_FILL_LEVEL + "/" + EMPTY;
        }
    }
}