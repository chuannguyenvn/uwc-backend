namespace Commons.RequestStatuses.Authentication
{
    public class InvalidRoleWhenLogin : BadRequest
    {
        public override string Result { get; protected set; } = "Invalid Role When Login";

        public override string Message { get; protected set; } =
            "You can only login on a desktop with a supervisor account, and on a mobile with a worker acocunt.";
    }
}