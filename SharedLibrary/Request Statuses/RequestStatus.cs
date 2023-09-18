namespace Commons.RequestStatuses
{
    public abstract class RequestStatus
    {
        public abstract HttpResponseStatusType StatusType { get; protected set; }
        public abstract string Result { get; protected set; }
        public abstract string Message { get; protected set; }
        public readonly DateTime CompletionTime = DateTime.Now;
    }
}