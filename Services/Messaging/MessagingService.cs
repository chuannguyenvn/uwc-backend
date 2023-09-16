using Commons.Communications.Messages;
using Commons.Models;
using Repositories;
using RequestStatuses;
using RequestStatuses.Authentication;

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

    public ParamRequestResult<GetMessagesBetweenTwoUsersResponse> GetMessagesBetweenTwoUsers(GetMessagesBetweenTwoUsersRequest request)
    {
        var messages = _unitOfWork.Messages.GetMessagesBetweenTwoUsers(request.SenderAccountID, request.ReceiverAccountID);
        var response = new GetMessagesBetweenTwoUsersResponse()
        {
            Messages = messages.ToList(),
        };
        return new ParamRequestResult<GetMessagesBetweenTwoUsersResponse>(new Success(), response);
    }

    public ParamRequestResult<GetPreviewMessagesResponse> GetPreviewMessages(GetPreviewMessagesRequest request)
    {
        if (!_unitOfWork.Accounts.DoesIdExist(request.UserAccountId))
            return new ParamRequestResult<GetPreviewMessagesResponse>(new UsernameNotExist());
        
        var messages = _unitOfWork.Messages.GetPreviewMessages(request.UserAccountId);
        var dictionary = new Dictionary<UserProfile, Message>();
        foreach (var message in messages)
        {
            if (message.SenderAccountID == request.UserAccountId)
            {
                dictionary.Add(message.ReceiverAccount.UserProfile, message);
            }
            else
            {
                dictionary.Add(message.SenderAccount.UserProfile, message);
            }
        }

        var response = new GetPreviewMessagesResponse()
        {
            PreviewMessagesByUserProfile = dictionary,
        };
        
        return new ParamRequestResult<GetPreviewMessagesResponse>(new Success(), response);
    }
}