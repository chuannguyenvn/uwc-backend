using Commons.Models;

namespace Commons.Communications.Messages;

public class GetPreviewMessagesRequest
{
    public int UserAccountId { get; set; }
}

public class GetPreviewMessagesResponse
{
    public Dictionary<UserProfile, Message> PreviewMessagesByUserProfile { get; set; }
}