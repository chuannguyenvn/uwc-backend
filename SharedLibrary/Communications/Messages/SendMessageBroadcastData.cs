using System.Collections.Generic;
using Commons.Models;

namespace Commons.Communications.Messages
{
    public class SendMessageBroadcastData
    {
        public List<Message> Messages { get; set; }
    }
}