namespace Services;

public struct RequestResult
{
    public bool Success { get; private set; }
    public string Message { get; private set; }
    public object Data { get; private set; }

    public RequestResult(bool success, string message, object data)
    {
        Success = success;
        Message = message;
        Data = data;
    }
}