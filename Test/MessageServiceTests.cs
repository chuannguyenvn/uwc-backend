using Repositories.Managers;
using Services.Messaging;
using Commons.Communications.Messages;
using Commons.RequestStatuses;
using Commons.RequestStatuses.Authentication;

namespace Test;

public class MessageServiceTests
{
    private MockUnitOfWork _mockUnitOfWork;
    
    [SetUp]
    public void Setup()
    {
        _mockUnitOfWork = new MockUnitOfWork();
    }

    [Test]
    public void SendMessage()
    {
        var messageService = new MessagingService(_mockUnitOfWork);

        var messageContent = "hello";

        var result = messageService.SendMessage(new SendMessageRequest()
        {
            SenderAccountID = _mockUnitOfWork.Accounts.GetById(1).UserProfileID,
            ReceiverAccountID = _mockUnitOfWork.Accounts.GetById(2).UserProfileID,
            Content = messageContent,
        });

        Assert.IsInstanceOf<Success>(result.RequestStatus);
    }

    [Test]
    public void SendAndGetMessages()
    {
        var messageService = new MessagingService(_mockUnitOfWork);

        var messageContent = "hello";

        messageService.SendMessage(new SendMessageRequest()
        {
            SenderAccountID = _mockUnitOfWork.Accounts.GetById(1).UserProfileID,
            ReceiverAccountID = _mockUnitOfWork.Accounts.GetById(2).UserProfileID,
            Content = messageContent,
        });

        var result = messageService.GetMessagesBetweenTwoUsers(new GetMessagesBetweenTwoUsersRequest()
        {
            SenderAccountID = _mockUnitOfWork.Accounts.GetById(1).UserProfileID,
            ReceiverAccountID = _mockUnitOfWork.Accounts.GetById(2).UserProfileID,
        });

        Assert.AreEqual(result.Data.Messages[0].Content, messageContent);
    }

    [Test]
    public void GetPreviewMessagesFromNewAccount()
    {
        var messageService = new MessagingService(_mockUnitOfWork);

        var result = messageService.GetPreviewMessages(new GetPreviewMessagesRequest()
        {
            UserAccountId = 1,
        });

        Assert.AreEqual(result.Data.PreviewMessagesByUserProfile.Count, 0);
    }

    [Test]
    public void GetPreviewMessagesFromImaginaryAccount()
    {
        var messageService = new MessagingService(_mockUnitOfWork);

        var result = messageService.GetPreviewMessages(new GetPreviewMessagesRequest()
        {
            UserAccountId = 9999999,
        });

        Assert.IsInstanceOf<UsernameNotExist>(result.RequestStatus);
    }
}