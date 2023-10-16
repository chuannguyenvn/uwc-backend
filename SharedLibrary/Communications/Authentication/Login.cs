using System.Text.Json.Serialization;

namespace Commons.Communications.Authentication
{
    public class LoginRequest
    {
        [JsonInclude] public string Username;
        [JsonInclude] public string Password;

        [JsonInclude] public bool IsFromDesktop;
        
        [JsonConstructor]
        public LoginRequest()
        {
        }
    }

    public class LoginResponse
    {
        [JsonInclude] public Credentials Credentials;
        [JsonInclude] public InitializationData InitializationData;

        [JsonConstructor]
        public LoginResponse()
        {
        }
    }
}