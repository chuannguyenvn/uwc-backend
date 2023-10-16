using System.Collections.Generic;

namespace Commons.Communications.Authentication
{
    public class InitializationData
    {
        public List<int> McpIds { get; set; }
        public List<double> McpLatitudes { get; set; }
        public List<double> McpLongitudes { get; set; }
    }
}