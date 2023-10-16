using Commons.Types;

namespace Commons.Communications.Map
{
    public class LocationUpdateRequest
    {
        public int AccountId { get; set; }
        public Coordinate NewLocation { get; set; }
    }
}