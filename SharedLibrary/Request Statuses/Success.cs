namespace Commons.RequestStatuses
{
    public class Success : RequestStatus
    {
        public override HttpResponseStatusType StatusType { get; protected set; } = HttpResponseStatusType.Ok;
        public override string Result { get; protected set; } = "Success";
        public override string Message { get; protected set; } = "Request completed successfully.";
    }
}