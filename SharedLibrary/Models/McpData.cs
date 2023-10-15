using Commons.Types;

namespace Commons.Models
{
    public class McpData : IndexedEntity
    {
        public string Address { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Coordinate Coordinate
        {
            get => new Coordinate(Latitude, Longitude);
            set
            {
                Latitude = value.Latitude;
                Longitude = value.Longitude;
            }
        }

        public Zone? Zone { get; set; } = null;

        public float Capacity { get; set; }
    }
}