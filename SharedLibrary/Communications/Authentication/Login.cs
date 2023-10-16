namespace Commons.Communications.Authentication
{
    public class LoginRequest
    {
#if NET7_0
        public string Username { get; set; }
        public string Password { get; set; }

        public bool IsFromDesktop { get; set; }
#else
        public string Username;
        public string Password;

        public bool IsFromDesktop;

        public LoginRequest()
        {
        }
#endif
    }

    public class LoginResponse
    {
        public Credentials Credentials { get; set; }
        public InitializationData InitializationData { get; set; }
    }
}