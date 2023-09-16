using Commons.Communications.Messages;
using Commons.Models;
using Repositories;
using RequestStatuses;

namespace Services.Messaging;

public class MessagingService : IMessagingService
{
    private readonly IUnitOfWork _unitOfWork;

    public MessagingService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public RequestResult SendMessage(SendMessageRequest request)
    {
        var message = new Message
        {
            SenderAccountID = request.SenderAccountID,
            ReceiverAccountID = request.ReceiverAccountID,
            Content = request.Content
        };
        _unitOfWork.Messages.Add(message);
        _unitOfWork.Complete();

        return new RequestResult(new Success());
    }

    public RequestResult GetMessagesBetweenTwoUsers(GetMessagesBetweenTwoUsersRequest request)
    {
        var messages = _unitOfWork.Messages.GetMessagesBetweenTwoUsers(request.SenderAccountID, request.ReceiverAccountID);
        return new RequestResult(new Success(), messages);
    }

    public RequestResult GetPreviewMessages(GetPreviewMessagesRequest request)
    {
        var messages = _unitOfWork.Messages.GetPreviewMessages(request.UserAccountId);
        return new RequestResult(new Success(), messages);
    }
}