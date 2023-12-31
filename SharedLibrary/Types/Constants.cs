namespace Commons.Types
{
    public static class Constants
    {
        private const string MAPBOX_ACCESS_TOKEN =
            "pk.eyJ1IjoiY2h1YW4tbmd1eWVudm4iLCJhIjoiY2xsYTkycjJoMGg1MjNxbGhhcW5mMzNuOCJ9.tpAt14HVH_j1IKuKxsK31A";

        public const string MAPBOX_DIRECTION_API =
            "https://api.mapbox.com/directions/v5/mapbox/driving-traffic/{0};{1}?geometries=geojson&steps=true&access_token=" + MAPBOX_ACCESS_TOKEN;

        public const string MAPBOX_MATRIX_API =
            "https://api.mapbox.com/directions-matrix/v1/mapbox/driving-traffic/{0}?access_token=" + MAPBOX_ACCESS_TOKEN;

        private const string OPEN_WEATHER_API_KEY = "6e086f4dd9b2ca2ea6093b17fca79d21";

        public const string OPEN_WEATHER_API =
            "https://api.openweathermap.org/data/3.0/onecall?lat=10.7730603&lon=106.6570281&exclude=minutely,alerts&units=metric&appid=" +
            OPEN_WEATHER_API_KEY;

        private const string FACIAL_RECOGNITION_API = "https://uwc-facial-recognition.azurewebsites.net/api/";

        public const string REGISTER_FACE_API = FACIAL_RECOGNITION_API + "register";
        public const string VERIFY_FACE_API = FACIAL_RECOGNITION_API + "verify";
    }
}