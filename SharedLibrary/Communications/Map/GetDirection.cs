using System.Collections.Generic;
using Commons.Models;
using Commons.Types;

namespace Commons.Communications.Map
{
    public class GetDirectionRequest
    {
        public Coordinate CurrentLocation { get; set; }
        public List<int> McpIds { get; set; }
    }

    public class GetDirectionResponse
    {
        public Direction Direction { get; set; }
    }
}