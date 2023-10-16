namespace Commons.RequestStatuses.Authentication
{
    public class CannotRecognizeFace : BadRequest
    {
        public override string Result { get; protected set; } = "Cannot Recognize Face";
        public override string Message { get; protected set; } = "The face you provided cannot be recognized.";
    }
}