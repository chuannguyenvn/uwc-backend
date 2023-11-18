namespace Commons.Endpoints
{
    public static partial class Endpoints
    {
        public static class VehicleData
        {
            private const string VEHICLE_DATA = "vehicledata";
            
            public const string ADD = "add";
            public const string UPDATE = "update";
            public const string REMOVE = "remove";
            public const string GET_ALL = "getall";
            
            public static string Add => DOMAIN + "/" + VEHICLE_DATA + "/" + ADD;
            public static string Update => DOMAIN + "/" + VEHICLE_DATA + "/" + UPDATE;
            public static string Remove => DOMAIN + "/" + VEHICLE_DATA + "/" + REMOVE;
            public static string GetAll => DOMAIN + "/" + VEHICLE_DATA + "/" + GET_ALL;
        }
    }
}