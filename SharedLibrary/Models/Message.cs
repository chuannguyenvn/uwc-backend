using System;
using Newtonsoft.Json;

namespace Commons.Models
{
    public class Message : IndexedEntity
    {
        public int SenderAccountId { get; set; }
        [JsonIgnore] public Account SenderAccount { get; set; }

        public int ReceiverAccountId { get; set; }
        [JsonIgnore] public Account ReceiverAccount { get; set; }

        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}