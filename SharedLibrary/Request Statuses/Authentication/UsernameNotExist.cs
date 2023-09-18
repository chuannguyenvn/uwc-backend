namespace Commons.RequestStatuses.Authentication
{
    public class UsernameNotExist : BadRequest
    {
        public override string Result { get; protected set; } = "Username Does Not Exist";
        public override string Message { get; protected set; } = "The username you entered does not exist.";
    }
}