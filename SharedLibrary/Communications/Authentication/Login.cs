namespace Commons.Communications.Authentication
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public bool IsFromDesktop { get; set; }
    }

    public class LoginResponse
    {
        public string JwtToken { get; set; }
    }
}