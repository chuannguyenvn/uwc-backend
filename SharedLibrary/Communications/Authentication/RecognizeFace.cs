namespace Commons.Communications.Authentication
{
    public class LoginWithFaceRequest
    {
        public int AccountId { get; set; }
        public object FaceImage { get; set; }
    }

    public class LoginWithFaceResponse
    {
        public bool IsValid { get; set; }
        public Credentials? Credentials { get; set; }
    }
}