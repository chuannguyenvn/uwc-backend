using System;

namespace Commons.Models
{
    public class Message : IndexedEntity
    {
        public int SenderProfileId { get; set; }
        public UserProfile SenderUserProfile { get; set; }

        public int ReceiverProfileId { get; set; }
        public UserProfile ReceiverUserProfile { get; set; }

        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsSeen { get; set; } = false;
    }
}