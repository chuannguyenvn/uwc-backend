namespace Commons.RequestStatuses
{
    public class DataEntryAlreadyExist : BadRequest
    {
        public override string Result { get; protected set; } = "Data Entry Already Exist";
        public override string Message { get; protected set; } = "The data entry you are trying to add already exist.";
    
    }
}