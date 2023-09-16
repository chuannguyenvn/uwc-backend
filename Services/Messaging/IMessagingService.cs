using Commons.Communications.Messages;

namespace Services.Messaging;

public interface IMessagingService
{
    public RequestResult SendMessage(SendMessageRequest request);
    public RequestResult GetMessagesBetweenTwoUsers(GetMessagesBetweenTwoUsersRequest request);
    public RequestResult GetPreviewMessages(GetPreviewMessagesRequest request);
}