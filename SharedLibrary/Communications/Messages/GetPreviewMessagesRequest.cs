using System.Collections.Generic;
using Commons.Models;

namespace Commons.Communications.Messages
{
    public class GetPreviewMessagesRequest
    {
        public int UserAccountId { get; set; }
    }

    public class GetPreviewMessagesResponse
    {
        public List<string> FullNames { get; set; }
        public List<Message> Messages { get; set; }
    }
}