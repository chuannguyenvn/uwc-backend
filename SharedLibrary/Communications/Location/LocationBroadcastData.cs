using System.Collections.Generic;
using Commons.Types;

namespace Commons.Communications.Location
{
    public class LocationBroadcastData
    {
        public Dictionary<int, Coordinate> LocationByIds { get; set; }
    }
}