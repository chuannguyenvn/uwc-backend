﻿using System.Collections.Generic;
using Commons.Models;

namespace Commons.Communications.Messages
{
    public class GetMessagesBetweenTwoUsersRequest
    {
        public int UserAccountId { get; set; }
        public int OtherUserAccountId { get; set; }
    }

    public class GetMessagesBetweenTwoUsersResponse
    {
        public List<Message> Messages { get; set; }
    }
}