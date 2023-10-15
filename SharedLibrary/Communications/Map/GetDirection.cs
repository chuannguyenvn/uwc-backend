using System.Collections.Generic;
using Commons.Types;

namespace Commons.Communications.Map
{
    public class GetDirectionRequest
    {
        public int AccountId { get; set; }
        public Coordinate CurrentLocation { get; set; }
        public List<Coordinate> Destinations { get; set; }
    }

    public class GetDirectionResponse
    {
        public RawMapboxDirectionResponse Direction { get; set; }
    }
}