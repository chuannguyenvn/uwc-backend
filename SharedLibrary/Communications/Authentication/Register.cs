namespace Commons.Communications.Authentication
{
    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RegisterResponse
    {
        public Credentials Credentials { get; set; }
        public InitializationData InitializationData { get; set; }
    }
}