namespace Commons.Endpoints
{
    public static partial class Endpoints
    {
        public static class UserProfile
        {
            private const string USER_PROFILE = "userprofile";
            
            public const string GET_USER_PROFILE = "get-user-profile/{id}";
            public const string GET_ALL_USER_PROFILES = "get-all-user-profiles";
            public const string GET_ALL_WORKER_PROFILES = "get-all-worker-profiles";
            public const string GET_ALL_DRIVER_PROFILES = "get-all-driver-profiles";
            public const string GET_ALL_CLEANER_PROFILES = "get-all-cleaner-profiles";
            public const string UPDATE_USER_PROFILE = "update-user-profile";
            
            public static string GetUserProfile => DOMAIN + "/" + USER_PROFILE + "/" + GET_USER_PROFILE;
            public static string GetAllUserProfiles => DOMAIN + "/" + USER_PROFILE + "/" + GET_ALL_USER_PROFILES;
            public static string GetAllWorkerProfiles => DOMAIN + "/" + USER_PROFILE + "/" + GET_ALL_WORKER_PROFILES;
            public static string GetAllDriverProfiles => DOMAIN + "/" + USER_PROFILE + "/" + GET_ALL_DRIVER_PROFILES;
            public static string GetAllCleanerProfiles => DOMAIN + "/" + USER_PROFILE + "/" + GET_ALL_CLEANER_PROFILES;
            public static string UpdateUserProfile => DOMAIN + "/" + USER_PROFILE + "/" + UPDATE_USER_PROFILE;
        }
    }
}