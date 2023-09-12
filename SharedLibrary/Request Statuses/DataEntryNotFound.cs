namespace RequestStatuses;

public class DataEntryNotFound : BadRequest
{
    public override string Result { get; protected set; } = "Data Entry Not Found";
    public override string Message { get; protected set; } = "The data entry you are looking for does not exist.";
}