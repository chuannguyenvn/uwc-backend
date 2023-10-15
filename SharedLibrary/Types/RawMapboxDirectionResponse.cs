using System.Collections.Generic;
using Newtonsoft.Json;

namespace Commons.Types
{
    public class Admin
    {
        [JsonProperty("iso_3166_1_alpha3")]
        public string Iso31661Alpha3 { get; set; }

        [JsonProperty("iso_3166_1")]
        public string Iso31661 { get; set; }
    }

    public class Geometry
    {
        [JsonProperty("coordinates")]
        public List<List<double>> Coordinates { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class Intersection
    {
        [JsonProperty("entry")]
        public List<bool> Entry { get; set; }

        [JsonProperty("bearings")]
        public List<int> Bearings { get; set; }

        [JsonProperty("duration")]
        public double Duration { get; set; }

        [JsonProperty("mapbox_streets_v8")]
        public MapboxStreetsV8 MapboxStreetsV8 { get; set; }

        [JsonProperty("is_urban")]
        public bool IsUrban { get; set; }

        [JsonProperty("admin_index")]
        public int AdminIndex { get; set; }

        [JsonProperty("out")]
        public int Out { get; set; }

        [JsonProperty("weight")]
        public double Weight { get; set; }

        [JsonProperty("geometry_index")]
        public int GeometryIndex { get; set; }

        [JsonProperty("location")]
        public List<double> Location { get; set; }

        [JsonProperty("in")]
        public int? In { get; set; }

        [JsonProperty("turn_weight")]
        public double? TurnWeight { get; set; }

        [JsonProperty("turn_duration")]
        public double? TurnDuration { get; set; }

        [JsonProperty("traffic_signal")]
        public bool? TrafficSignal { get; set; }

        [JsonProperty("lanes")]
        public List<Lane> Lanes { get; set; }

        [JsonProperty("classes")]
        public List<string> Classes { get; set; }

        [JsonProperty("toll_collection")]
        public TollCollection TollCollection { get; set; }
    }

    public class Lane
    {
        [JsonProperty("indications")]
        public List<string> Indications { get; set; }

        [JsonProperty("valid_indication")]
        public string ValidIndication { get; set; }

        [JsonProperty("valid")]
        public bool Valid { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }
    }

    public class Leg
    {
        [JsonProperty("via_waypoints")]
        public List<object> ViaWaypoints { get; set; }

        [JsonProperty("admins")]
        public List<Admin> Admins { get; set; }

        [JsonProperty("weight")]
        public double Weight { get; set; }

        [JsonProperty("duration")]
        public double Duration { get; set; }

        [JsonProperty("steps")]
        public List<Step> Steps { get; set; }

        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }
    }

    public class Maneuver
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("instruction")]
        public string Instruction { get; set; }

        [JsonProperty("bearing_after")]
        public int BearingAfter { get; set; }

        [JsonProperty("bearing_before")]
        public int BearingBefore { get; set; }

        [JsonProperty("location")]
        public List<double> Location { get; set; }

        [JsonProperty("modifier")]
        public string Modifier { get; set; }
    }

    public class MapboxStreetsV8
    {
        [JsonProperty("class")]
        public string Class { get; set; }
    }

    public class RawMapboxDirectionResponse
    {
        [JsonProperty("routes")]
        public List<Route> Routes { get; set; }

        [JsonProperty("waypoints")]
        public List<Waypoint> Waypoints { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }
    }

    public class Route
    {
        [JsonProperty("weight_name")]
        public string WeightName { get; set; }

        [JsonProperty("weight")]
        public double Weight { get; set; }

        [JsonProperty("duration")]
        public double Duration { get; set; }

        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("legs")]
        public List<Leg> Legs { get; set; }

        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }
    }

    public class Step
    {
        [JsonProperty("intersections")]
        public List<Intersection> Intersections { get; set; }

        [JsonProperty("maneuver")]
        public Maneuver Maneuver { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("duration")]
        public double Duration { get; set; }

        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("driving_side")]
        public string DrivingSide { get; set; }

        [JsonProperty("weight")]
        public double Weight { get; set; }

        [JsonProperty("mode")]
        public string Mode { get; set; }

        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }

        [JsonProperty("destinations")]
        public string Destinations { get; set; }

        [JsonProperty("ref")]
        public string Ref { get; set; }

        [JsonProperty("exits")]
        public string Exits { get; set; }
    }

    public class TollCollection
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class Waypoint
    {
        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("location")]
        public List<double> Location { get; set; }
    }


}