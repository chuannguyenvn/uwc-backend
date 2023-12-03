namespace Commons.Types
{
    public static class Constants
    {
        private const string MAPBOX_ACCESS_TOKEN =
            "pk.eyJ1IjoiY2h1YW4tbmd1eWVudm4iLCJhIjoiY2xsYTkycjJoMGg1MjNxbGhhcW5mMzNuOCJ9.tpAt14HVH_j1IKuKxsK31A";

        public const string MAPBOX_DIRECTION_API =
            "https://api.mapbox.com/directions/v5/mapbox/driving-traffic/{0};{1}?geometries=geojson&access_token=" + MAPBOX_ACCESS_TOKEN;
        
        public const string MAPBOX_MATRIX_API = "https://api.mapbox.com/directions-matrix/v1/mapbox/driving-traffic/{0}?access_token=" + MAPBOX_ACCESS_TOKEN;
    }
}