namespace Commons.Endpoints
{
    public static partial class Endpoints
    {
        public static class Messaging
        {
            private const string MESSAGING = "messaging";
            
            public const string SEND_MESSAGE = "send";
            public const string GET_MESSAGES_BETWEEN_TWO_USERS = "get-messages-between-two-users";
            public const string GET_PREVIEW_MESSAGES = "get-preview-messages";
            
            public static string SendMessage => DOMAIN + "/" + MESSAGING + "/" + SEND_MESSAGE;
            public static string GetMessagesBetweenTwoUsers => DOMAIN + "/" + MESSAGING + "/" + GET_MESSAGES_BETWEEN_TWO_USERS;
            public static string GetPreviewMessages => DOMAIN + "/" + MESSAGING + "/" + GET_PREVIEW_MESSAGES;
        }
    }
}