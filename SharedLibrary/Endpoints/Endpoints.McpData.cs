namespace Commons.Endpoints
{
    public static partial class Endpoints
    {
        public static class McpData
        {
            private const string MCP_DATA = "mcpdata";

            public const string ADD = "add";
            public const string UPDATE = "update";
            public const string REMOVE = "remove";
            public const string GET = "get";

            public static string Add => DOMAIN + "/" + MCP_DATA + "/" + ADD;
            public static string Update => DOMAIN + "/" + MCP_DATA + "/" + UPDATE;
            public static string Remove => DOMAIN + "/" + MCP_DATA + "/" + REMOVE;
            public static string Get => DOMAIN + "/" + MCP_DATA + "/" + GET;
        }
    }
}