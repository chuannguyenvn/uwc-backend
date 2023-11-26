using System.Collections.Generic;
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
        
        public ICollection<McpFillLevelLog> McpFillLevelLogs { get; set; }
        public ICollection<McpEmptyRecord> McpEmptyRecords { get; set; }
    }
}