namespace Commons.RequestStatuses
{
    public class InvalidInputData : BadRequest
    {
        public override string Result { get; protected set; } = "Invalid Input Data";
        public override string Message { get; protected set; } = "The input data you provided is invalid.";
    }
}