using System.Collections.Generic;

namespace Commons.Communications.Authentication
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public bool IsFromDesktop { get; set; }
    }

    public class LoginWithFaceRequest
    {
        public string Username { get; set; }
        public List<byte[]> Images { get; set; }
    }

    public class LoginResponse
    {
        public Credentials Credentials { get; set; }
        public InitializationData InitializationData { get; set; }
    }
}