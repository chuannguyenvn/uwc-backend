using System.Collections.Generic;
using Newtonsoft.Json;

namespace Commons.Types
{
    public class Destination
    {
        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("location")] public List<double> Location { get; set; }
        public Coordinate Coordinate => new Coordinate(Location[1], Location[0]);

        [JsonProperty("distance")] public float Distance { get; set; }
    }

    public class MapboxMatrixResponse
    {
        [JsonProperty("code")] public string Code { get; set; }

        [JsonProperty("durations")] public List<List<double>> Durations { get; set; }

        [JsonProperty("destinations")] public List<Destination> Destinations { get; set; }

        [JsonProperty("sources")] public List<Source> Sources { get; set; }
    }

    public class Source
    {
        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("location")] public List<double> Location { get; set; }
        public Coordinate Coordinate => new Coordinate(Location[1], Location[0]);

        [JsonProperty("distance")] public float Distance { get; set; }
    }
}