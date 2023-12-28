using System.Collections.Generic;
using Newtonsoft.Json;

namespace Commons.Types
{
    public class Current
    {
        [JsonProperty("dt")] public int Dt { get; set; }

        [JsonProperty("temp")] public double Temp { get; set; }
    }

    public class Daily
    {
        [JsonProperty("dt")] public int Dt { get; set; }

        [JsonProperty("pop")] public double Pop { get; set; }
    }

    public class Hourly
    {
        [JsonProperty("dt")] public int Dt { get; set; }

        [JsonProperty("pop")] public double Pop { get; set; }
    }

    public class OpenWeatherResponse
    {
        [JsonProperty("lat")] public double Lat { get; set; }

        [JsonProperty("lon")] public double Lon { get; set; }

        [JsonProperty("timezone")] public string Timezone { get; set; }

        [JsonProperty("timezone_offset")] public int TimezoneOffset { get; set; }

        [JsonProperty("current")] public Current Current { get; set; }

        [JsonProperty("hourly")] public List<Hourly> Hourly { get; set; }

        [JsonProperty("daily")] public List<Daily> Daily { get; set; }
    }
}