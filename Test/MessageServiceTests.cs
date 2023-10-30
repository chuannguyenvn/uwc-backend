using Repositories.Managers;
using Services.Messaging;
using Commons.Communications.Messages;
using Commons.RequestStatuses;
using Commons.RequestStatuses.Authentication;
using Hubs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.SignalR;
using Moq;

namespace Test;

public class MessageServiceTests
{
    private Mock<IHubContext<BaseHub>> _mockBaseHub = new Mock<IHubContext<BaseHub>>();

    [SetUp]
    public void Setup()
    {
        _mockBaseHub.Setup(mock => mock.Clients.Client(It.IsAny<string>()).SendCoreAsync(It.IsAny<string>(), It.IsAny<object[]>(), default));
    }

    [Test]
    public void SendMessage()
    {
        var mockUnitOfWork = new MockUnitOfWork();
        var messageService = new MessagingService(mockUnitOfWork, _mockBaseHub.Object);

        var messageContent = "hello";

        var result = messageService.SendMessage(new SendMessageRequest()
        {
            SenderAccountId = mockUnitOfWork.AccountRepository.GetById(1).UserProfileId,
            ReceiverAccountId = mockUnitOfWork.AccountRepository.GetById(2).UserProfileId,
            Content = messageContent,
        });

        Assert.IsInstanceOf<Success>(result.RequestStatus);
    }

    [Test]
    public void SendAndGetMessages()
    {
        var mockUnitOfWork = new MockUnitOfWork();
        var messageService = new MessagingService(mockUnitOfWork, _mockBaseHub.Object);

        var messageContent = "hello";

        messageService.SendMessage(new SendMessageRequest()
        {
            SenderAccountId = mockUnitOfWork.AccountRepository.GetById(1).UserProfileId,
            ReceiverAccountId = mockUnitOfWork.AccountRepository.GetById(2).UserProfileId,
            Content = messageContent,
        });

        var result = messageService.GetMessagesBetweenTwoUsers(new GetMessagesBetweenTwoUsersRequest()
        {
            UserAccountId = mockUnitOfWork.AccountRepository.GetById(1).UserProfileId,
            OtherUserAccountId = mockUnitOfWork.AccountRepository.GetById(2).UserProfileId,
        });

        Assert.AreEqual(result.Data.Messages[0].Content, messageContent);
    }

    [Test]
    public void GetPreviewMessagesFromNewAccount()
    {
        var mockUnitOfWork = new MockUnitOfWork();
        var messageService = new MessagingService(mockUnitOfWork, _mockBaseHub.Object);

        var result = messageService.GetPreviewMessages(new GetPreviewMessagesRequest()
        {
            UserAccountId = 20,
        });

        Assert.AreEqual(result.Data.FullNames.Count, 0);
    }

    [Test]
    public void GetPreviewMessagesFromNonExistentAccount()
    {
        var mockUnitOfWork = new MockUnitOfWork();
        var messageService = new MessagingService(mockUnitOfWork, _mockBaseHub.Object);

        var result = messageService.GetPreviewMessages(new GetPreviewMessagesRequest()
        {
            UserAccountId = 9999999,
        });

        Assert.IsInstanceOf<UsernameNotExist>(result.RequestStatus);
    }
}