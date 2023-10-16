using System.Collections.Generic;

namespace Commons.Communications.Authentication
{
    public class RegisterFaceRequest
    {
        public int AccountId { get; set; }
        public List<object> FaceImages { get; set; }
    }
}