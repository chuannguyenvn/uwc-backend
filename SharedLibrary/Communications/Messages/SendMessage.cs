namespace Commons.Communications.Messages
{
    public class SendMessageRequest
    {
        public int SenderAccountId { get; set; }
        public int ReceiverAccountId { get; set; }
        public string Content { get; set; }
    }
}