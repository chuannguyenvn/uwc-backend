using Commons.Types;

namespace Commons.Models
{
    public class McpData : IndexedEntity
    {
        public string Address { get; set; }
        public Coordinate Coordinate { get; set; }
        public Zone Zone { get; set; }
        public float Capacity { get; set; }
    }
}