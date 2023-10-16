using System.Collections.Generic;
using Commons.Types;

namespace Commons.Communications.Map
{
    public class WorkerLocationBroadcastData
    {
        public Dictionary<int, Coordinate> LocationByIds { get; set; }
    }
}