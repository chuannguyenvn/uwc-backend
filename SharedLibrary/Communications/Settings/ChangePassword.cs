namespace Commons.Communications.Settings
{
    public class ChangePasswordRequest
    {
        public int AccountId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
    
    public class ChangePasswordResponse
    {
        public bool Success { get; set; }
    }
}