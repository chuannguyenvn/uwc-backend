namespace Commons.RequestStatuses.Authentication;

public class IncorrectPassword : BadRequest
{
    public override string Result { get; protected set; } = "Incorrect Password";
    public override string Message { get; protected set; } = "The password you entered is incorrect.";
}