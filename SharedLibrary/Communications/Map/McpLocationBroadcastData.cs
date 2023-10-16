using System.Collections.Generic;
using Commons.Types;

namespace Commons.Communications.Map
{
    public class McpLocationBroadcastData
    {
        public Dictionary<int, Coordinate> LocationByIds { get; set; }
    }
}