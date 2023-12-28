namespace Commons.Communications.Messages
{
    public class ReadAllMessagesRequest
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
    }
}