using Commons.Types;

namespace Commons.Communications.Map
{
    public class GetLocationRequest
    {
        public int AccountId { get; set; }
    }

    public class GetLocationResponse
    {
        public Coordinate Coordinate { get; set; }
    }
}