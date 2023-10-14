using System;
#if NET7_0
using System.Text.Json.Serialization;
#endif

namespace Commons.Models
{
    public class Message : IndexedEntity
    {
        public int SenderAccountId { get; set; }

#if NET7_0
        [JsonIgnore]
#endif
        public Account SenderAccount { get; set; }

        public int ReceiverAccountId { get; set; }

#if NET7_0
        [JsonIgnore]
#endif
        public Account ReceiverAccount { get; set; }

        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}