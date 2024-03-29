﻿using Commons.Communications.Messages;

namespace Services.Messaging;

public interface IMessagingService
{
    public RequestResult SendMessage(SendMessageRequest request);
    public RequestResult ReadMessage(ReadAllMessagesRequest request);
    public ParamRequestResult<GetMessagesBetweenTwoUsersResponse> GetMessagesBetweenTwoUsers(GetMessagesBetweenTwoUsersRequest request);
    public ParamRequestResult<GetPreviewMessagesResponse> GetPreviewMessages(GetPreviewMessagesRequest request);
}