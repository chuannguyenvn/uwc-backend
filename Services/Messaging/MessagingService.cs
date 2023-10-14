using Repositories.Managers;
using Commons.Communications.Messages;
using Commons.HubHandlers;
using Commons.Models;
using Commons.RequestStatuses;
using Commons.RequestStatuses.Authentication;
using Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Services.Messaging;

public class MessagingService : IMessagingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHubContext<BaseHub> _hubContext;

    public MessagingService(IUnitOfWork unitOfWork, IHubContext<BaseHub> hubContext)
    {
        _unitOfWork = unitOfWork;
        _hubContext = hubContext;
    }

    public RequestResult SendMessage(SendMessageRequest request)
    {
        var message = new Message
        {
            SenderAccountId = request.SenderAccountID,
            ReceiverAccountId = request.ReceiverAccountID,
            Content = request.Content
        };
        _unitOfWork.Messages.Add(message);
        _unitOfWork.Complete();

        _hubContext.Clients.User(request.ReceiverAccountId.ToString())
            .SendAsync(HubHandlers.Messaging.SEND_MESSAGE, new SendMessageBroadcastData()
            {
                NewMessage = message,
            });

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

        var fullNames = new List<string>();
        var messages = _unitOfWork.Messages.GetPreviewMessages(request.UserAccountId).ToList();
        foreach (var message in messages)
        {
            var account = _unitOfWork.Accounts.GetById(message.SenderAccountId == request.UserAccountId
                ? message.ReceiverAccountId
                : message.SenderAccountId);
            var userProfile = _unitOfWork.UserProfiles.GetById(account.UserProfileID);
            var fullName = userProfile.FirstName + " " + userProfile.LastName;

            fullNames.Add(fullName);
        }

        var response = new GetPreviewMessagesResponse()
        {
            FullNames = fullNames,
            Messages = messages,
        };

        return new ParamRequestResult<GetPreviewMessagesResponse>(new Success(), response);
    }
}