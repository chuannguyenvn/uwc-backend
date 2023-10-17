using System.Collections.Generic;
using Commons.Types;

namespace Commons.Communications.Map
{
    public class WorkerLocationBroadcastData
    {
        public Dictionary<int, Coordinate> DriverLocationByIds { get; set; }
        public Dictionary<int, Coordinate> CleanerLocationByIds { get; set; }
    }
}