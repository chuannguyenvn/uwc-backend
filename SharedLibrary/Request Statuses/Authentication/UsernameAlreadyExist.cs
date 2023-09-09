namespace RequestStatuses.Authentication;

public class UsernameAlreadyExist : BadRequest
{
    public override string Result { get; protected set; } ="Username Already Exist";
    public override string Message { get; protected set; } = "The username you entered already exist.";
}