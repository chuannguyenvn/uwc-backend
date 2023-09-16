namespace Commons.Communications.Messages;

public class SendMessageRequest
{
    public int SenderAccountID { get; set; }
    public int ReceiverAccountID { get; set; }
    public string Content { get; set; }
}