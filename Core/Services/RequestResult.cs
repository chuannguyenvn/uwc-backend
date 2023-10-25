using Commons.RequestStatuses;

namespace Services;

public class RequestResult
{
    public RequestStatus RequestStatus { get; private set; }
    public object? Payload { get; protected set; }
    
    public RequestResult(RequestStatus requestStatus)
    {
        RequestStatus = requestStatus;
    }
}