using Commons.Types;

namespace Commons.Models;

public class McpInformation : IndexedEntity
{
    public string Address { get; set; }
    public Coordinate Coordinate { get; set; }
    public float Capacity { get; set; }
}