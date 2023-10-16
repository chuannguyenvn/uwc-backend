using System.Text.Json.Serialization;

namespace Commons.Communications.Authentication
{
    public class Credentials
    {
        [JsonInclude] public string JwtToken;
        [JsonInclude] public int AccountId;
        
        [JsonConstructor]
        public Credentials()
        {
        }
    }
}