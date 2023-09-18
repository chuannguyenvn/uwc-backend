using System.Collections.Generic;
using Commons.Models;

namespace Commons.Communications.Messages
{
    public class GetMessagesBetweenTwoUsersRequest
    {
        public int SenderAccountID { get; set; }
        public int ReceiverAccountID { get; set; }
    }

    public class GetMessagesBetweenTwoUsersResponse
    {
        public List<Message> Messages { get; set; }
    }
}