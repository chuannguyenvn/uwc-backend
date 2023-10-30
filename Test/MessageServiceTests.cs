using Repositories.Managers;
using Services.Messaging;
using Commons.Communications.Messages;
using Commons.RequestStatuses;
using Commons.RequestStatuses.Authentication;
using Hubs;
using Microsoft.AspNetCore.SignalR;
using Moq;

namespace Test;

public class MessageServiceTests
{
    private MockUnitOfWork _mockUnitOfWork;
    private Mock<IHubContext<BaseHub>> _mockBaseHub = new Mock<IHubContext<BaseHub>>();
    
    [SetUp]
    public void Setup()
    {
        _mockUnitOfWork = new MockUnitOfWork();
    }

    [Test]
    public void SendMessage()
    {
        var messageService = new MessagingService(_mockUnitOfWork , _mockBaseHub.Object);

        var messageContent = "hello";

        var result = messageService.SendMessage(new SendMessageRequest()
        {
            SenderAccountId = _mockUnitOfWork.AccountRepository.GetById(1).UserProfileId,
            ReceiverAccountId = _mockUnitOfWork.AccountRepository.GetById(2).UserProfileId,
            Content = messageContent,
        });

        Assert.IsInstanceOf<Success>(result.RequestStatus);
    }

    [Test]
    public void SendAndGetMessages()
    {
        var messageService = new MessagingService(_mockUnitOfWork , _mockBaseHub.Object);

        var messageContent = "hello";

        messageService.SendMessage(new SendMessageRequest()
        {
            SenderAccountId = _mockUnitOfWork.AccountRepository.GetById(1).UserProfileId,
            ReceiverAccountId = _mockUnitOfWork.AccountRepository.GetById(2).UserProfileId,
            Content = messageContent,
        });

        var result = messageService.GetMessagesBetweenTwoUsers(new GetMessagesBetweenTwoUsersRequest()
        {
            UserAccountId = _mockUnitOfWork.AccountRepository.GetById(1).UserProfileId,
            OtherUserAccountId = _mockUnitOfWork.AccountRepository.GetById(2).UserProfileId,
        });

        Assert.AreEqual(result.Data.Messages[0].Content, messageContent);
    }

    [Test]
    public void GetPreviewMessagesFromNewAccount()
    {
        var messageService = new MessagingService(_mockUnitOfWork , _mockBaseHub.Object);

        var result = messageService.GetPreviewMessages(new GetPreviewMessagesRequest()
        {
            UserAccountId = 1,
        });

        Assert.AreEqual(result.Data.FullNames.Count, 0);
    }

    [Test]
    public void GetPreviewMessagesFromImaginaryAccount()
    {
        var messageService = new MessagingService(_mockUnitOfWork , _mockBaseHub.Object);

        var result = messageService.GetPreviewMessages(new GetPreviewMessagesRequest()
        {
            UserAccountId = 9999999,
        });

        Assert.IsInstanceOf<UsernameNotExist>(result.RequestStatus);
    }
}