namespace Requests
{
    public static partial class Endpoints
    {
        public static class Authentication
        {
            private const string AUTHENTICATION = "authentication";

            public const string LOGIN = "login";
            public const string REGISTER = "register";

            public static string Login => DOMAIN + "/" + AUTHENTICATION + "/" + LOGIN;
            public static string Register => DOMAIN + "/" + AUTHENTICATION + "/" + REGISTER;
        }
    }
}