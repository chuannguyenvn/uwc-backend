using RequestStatuses;

namespace Services;

public class ParamRequestResult<T> : RequestResult
{
    public T Data { get; protected set; }

    public ParamRequestResult(RequestStatus requestStatus, T data) : base(requestStatus)
    {
        Data = data;
        Payload = data;
    }
}