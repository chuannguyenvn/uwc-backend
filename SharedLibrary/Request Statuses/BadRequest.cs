namespace Commons.RequestStatuses;

public abstract class BadRequest : RequestStatus
{
    public override HttpResponseStatusType StatusType { get; protected set; } = HttpResponseStatusType.BadRequest;
}