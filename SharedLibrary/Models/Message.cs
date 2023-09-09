namespace SharedLibrary.Models;

public class Message
{
    public int SenderAccountID { get; set; }
    public Account SenderAccount { get; set; }
    
    public int ReceiverAccountID { get; set; }
    public Account ReceiverAccount { get; set; }
    
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
}