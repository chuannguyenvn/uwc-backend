using System.Collections.Generic;
using Commons.Types;

namespace Commons.Communications.Authentication
{
    public class InitializationData
    {
        public Dictionary<int, Coordinate> McpLocationByIds { get; set; }
        public List<int> OnlineAccountIds { get; set; }
    }
}