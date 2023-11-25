using System.Collections.Generic;

namespace Commons.Communications.Authentication
{
    public class RegisterFaceRequest
    {
        public int AccountId { get; set; }
        public List<byte[]> Images { get; set; }
    }

    public class RegisterFaceResponse
    {
        public bool Success { get; set; }
    }
}