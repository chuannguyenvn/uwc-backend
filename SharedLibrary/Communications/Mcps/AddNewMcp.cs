using Commons.Types;

namespace Commons.Communications.Mcps
{
    public class AddNewMcpRequest
    {
        public string Address { get; set; }
        public Coordinate Coordinate { get; set; }
        public float Capacity { get; set; }
    }
}