namespace Requests
{
    public static partial class Endpoints
    {
        public static class Authentication
        {
            private const string AUTHENTICATION = "authentication";

            public const string LOGIN = "login";
            public const string REGISTER = "register";
            public const string LOGIN_WITH_FACE = "login-with-face";
            public const string REGISTER_FACE = "register-face";
            public const string DELETE_FACE = "delete-face";

            public static string Login => DOMAIN + "/" + AUTHENTICATION + "/" + LOGIN;
            public static string Register => DOMAIN + "/" + AUTHENTICATION + "/" + REGISTER;
            public static string LoginWithFace => DOMAIN + "/" + AUTHENTICATION + "/" + LOGIN_WITH_FACE;
            public static string RegisterFace => DOMAIN + "/" + AUTHENTICATION + "/" + REGISTER_FACE;
            public static string DeleteFace => DOMAIN + "/" + AUTHENTICATION + "/" + DELETE_FACE;
        }
    }
}