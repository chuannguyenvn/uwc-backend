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
            SenderProfileId = request.SenderAccountId,
            ReceiverProfileId = request.ReceiverAccountId,
            Content = request.Content,
            Timestamp = DateTime.Now,
        };
        _unitOfWork.MessageRepository.Add(message);
        _unitOfWork.Complete();

        if (BaseHub.ConnectionIds.TryGetValue(request.ReceiverAccountId, out var id))
            _hubContext.Clients.Client(id)
                .SendAsync(HubHandlers.Messaging.SEND_MESSAGE, new SendMessageBroadcastData()
                {
                    NewMessage = message,
                });

        return new RequestResult(new Success());
    }

    public ParamRequestResult<GetMessagesBetweenTwoUsersResponse> GetMessagesBetweenTwoUsers(GetMessagesBetweenTwoUsersRequest request)
    {
        var messages = _unitOfWork.MessageRepository.GetMessagesBetweenTwoUsers(request.UserAccountId, request.OtherUserAccountId);
        var response = new GetMessagesBetweenTwoUsersResponse()
        {
            Messages = messages.ToList(),
        };
        return new ParamRequestResult<GetMessagesBetweenTwoUsersResponse>(new Success(), response);
    }

    public ParamRequestResult<GetPreviewMessagesResponse> GetPreviewMessages(GetPreviewMessagesRequest request)
    {
        if (!_unitOfWork.AccountRepository.DoesIdExist(request.UserAccountId))
            return new ParamRequestResult<GetPreviewMessagesResponse>(new UsernameNotExist());

        var fullNames = new List<string>();
        var messages = _unitOfWork.MessageRepository.GetPreviewMessages(request.UserAccountId).ToList();
        foreach (var message in messages)
        {
            var account = _unitOfWork.AccountRepository.GetById(message.SenderProfileId == request.UserAccountId
                ? message.ReceiverProfileId
                : message.SenderProfileId);
            var userProfile = _unitOfWork.UserProfileRepository.GetById(account.UserProfileId);
            var fullName = userProfile.FirstName + " " + userProfile.LastName;

            fullNames.Add(fullName);

            message.SenderUserProfile = _unitOfWork.UserProfileRepository.GetById(message.SenderProfileId);
            message.ReceiverUserProfile = _unitOfWork.UserProfileRepository.GetById(message.ReceiverProfileId);

            message.SenderUserProfile.Account = null;
            message.ReceiverUserProfile.Account = null;
        }

        var response = new GetPreviewMessagesResponse()
        {
            FullNames = fullNames,
            Messages = messages,
        };

        return new ParamRequestResult<GetPreviewMessagesResponse>(new Success(), response);
    }
}