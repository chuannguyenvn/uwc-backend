using Commons.Models;
using Commons.Types;

namespace Commons.Communications.Mcps
{
    public class UpdateMcpRequest
    {
        public int McpId { get; set; }
        public string? NewAddress { get; set; }
        public Coordinate? NewCoordinate { get; set; }
        public float? NewCapacity { get; set; }
    }
}