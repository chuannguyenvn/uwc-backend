namespace Commons.Endpoints
{
    public static partial class Endpoints
    {
        public static class Map
        {
            private const string MAP = "map";

            public const string GET_LOCATION = "get-location";
            public const string UPDATE_LOCATION = "update-location";
            public const string GET_DIRECTION = "get-direction";
            
            public static string GetLocation => DOMAIN + "/" + MAP + "/" + GET_LOCATION;
            public static string UpdateLocation => DOMAIN + "/" + MAP + "/" + UPDATE_LOCATION;
            public static string GetDirection => DOMAIN + "/" + MAP + "/" + GET_DIRECTION;
        }
    }
}