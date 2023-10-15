using System.Collections.Generic;
using Commons.Types;

namespace Commons.Communications.Map
{
    public class LocationBroadcastData
    {
        public Dictionary<int, Coordinate> LocationByIds { get; set; }
    }
}