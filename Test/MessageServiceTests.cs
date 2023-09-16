using Commons.Communications.Messages;
using Commons.Models;
using Repositories;
using Services.Messaging;

namespace Test;

public class MessageServiceTests
{
    private MockUnitOfWork _mockUnitOfWork;

    private Account _senderAccount = new Account()
    {
        Username = "sender",
        PasswordHash = "password",
        PasswordSalt = "salt",
        UserProfileID = 1,
    };
    
    private Account _receiverAccount = new Account()
    {
        Username = "receiver",
        PasswordHash = "password",
        PasswordSalt = "salt",
        UserProfileID = 2,
    };
    
    [SetUp]
    public void Setup()
    {
        _mockUnitOfWork = new MockUnitOfWork();
        _mockUnitOfWork.Accounts.Add(_senderAccount);
        _mockUnitOfWork.Accounts.Add(_receiverAccount);
    }

    [Test]
    public void SendMessage()
    {
        var messageService = new MessagingService(_mockUnitOfWork);

        var messageContent = "hello";

        messageService.SendMessage(new SendMessageRequest()
        {
            SenderAccountID = _senderAccount.UserProfileID,
            ReceiverAccountID = _receiverAccount.UserProfileID,
            Content = messageContent,
        });

        var result = messageService.GetMessagesBetweenTwoUsers(new GetMessagesBetweenTwoUsersRequest()
        {
            SenderAccountID = _senderAccount.UserProfileID,
            ReceiverAccountID = _receiverAccount.UserProfileID,
        });

        Assert.AreEqual((result.Data as IEnumerable<Message>).ToList()[0].Content, messageContent);
    }
}