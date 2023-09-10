using RequestStatuses;

namespace Services;

public class RequestResult
{
    public RequestStatus RequestStatus { get; private set; }
    public object? Data { get; private set; }
    
    public RequestResult(RequestStatus requestStatus, object? data = null)
    {
        RequestStatus = requestStatus;
        Data = data;
    }
}