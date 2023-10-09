using System;

namespace Commons.Models
{
    public class Message : IndexedEntity
    {
        public int SenderAccountId { get; set; }
        public Account SenderAccount { get; set; }
    
        public int ReceiverAccountId { get; set; }
        public Account ReceiverAccount { get; set; }
    
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}