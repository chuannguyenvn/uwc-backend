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
        Console.WriteLine("SendMessage: " + request.Content);
        
        var message = new Message
        {
            SenderProfileId = request.SenderAccountId,
            ReceiverProfileId = request.ReceiverAccountId,
            Content = request.Content,
            Timestamp = DateTime.UtcNow,
        };
        _unitOfWork.MessageRepository.Add(message);
        _unitOfWork.Complete();
        
        var messages = GetMessagesBetweenTwoUsers(new GetMessagesBetweenTwoUsersRequest
        {
            UserAccountId = request.SenderAccountId,
            OtherUserAccountId = request.ReceiverAccountId,
            CurrentMessageCount = 0
        }).Data.Messages;

        if (BaseHub.ConnectionIds.TryGetValue(request.ReceiverAccountId, out var id))
            _hubContext.Clients.Client(id)
                .SendAsync(HubHandlers.Messaging.SEND_MESSAGE, new SendMessageBroadcastData()
                {
                    Messages = messages,
                });

        if (BaseHub.ConnectionIds.TryGetValue(request.SenderAccountId, out var id2))
            _hubContext.Clients.Client(id2)
                .SendAsync(HubHandlers.Messaging.SEND_MESSAGE, new SendMessageBroadcastData()
                {
                    Messages = messages,
                });
        
        return new RequestResult(new Success());
    }

    public RequestResult ReadMessage(ReadAllMessagesRequest request)
    {
        var messages = _unitOfWork.MessageRepository.GetMessagesBetweenTwoUsers(request.SenderId, request.ReceiverId).ToList();

        foreach (var message in messages)
        {
            if (message.ReceiverProfileId == request.ReceiverId) message.IsSeen = true;
        }

        _unitOfWork.Complete();

        if (BaseHub.ConnectionIds.TryGetValue(request.SenderId, out var id))
            _hubContext.Clients.Client(id)
                .SendAsync(HubHandlers.Messaging.READ_MESSAGE, new ReadAllMessagesBroadcastData()
                {
                    ReceiverId = request.ReceiverId,
                });

        return new RequestResult(new Success());
    }

    public ParamRequestResult<GetMessagesBetweenTwoUsersResponse> GetMessagesBetweenTwoUsers(GetMessagesBetweenTwoUsersRequest request)
    {
        var messages = _unitOfWork.MessageRepository.GetMessagesBetweenTwoUsers(request.UserAccountId, request.OtherUserAccountId).ToList();

        foreach (var message in messages)
        {
            if (message.ReceiverProfileId == request.UserAccountId) message.IsSeen = true;
        }

        _unitOfWork.Complete();

        var response = new GetMessagesBetweenTwoUsersResponse()
        {
            Messages = messages.Skip(request.CurrentMessageCount).Take(20).ToList(),
            IsContinuous = request.CurrentMessageCount > 0,
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