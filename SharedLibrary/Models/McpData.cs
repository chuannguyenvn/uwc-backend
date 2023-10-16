using Commons.Types;

#if NET7_0
using System.ComponentModel.DataAnnotations.Schema;
#endif

namespace Commons.Models
{
    public class McpData : IndexedEntity
    {
        public string Address { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

#if NET7_0
        [NotMapped]
#endif
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