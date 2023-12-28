using Repositories.Generics;
using Repositories.Managers;
using Commons.Models;

namespace Repositories.Implementations.Messages;

public class MockMessageRepository : MockGenericRepository<Message>, IMessageRepository
{
    public MockMessageRepository(MockUwcDbContext mockUwcDbContext) : base(mockUwcDbContext)
    {
    }

    public IEnumerable<Message> GetMessagesBetweenTwoUsers(int userAccountId, int otherUserAccountId)
    {
        return Context.MessageTable.Where(m => (m.SenderProfileId == userAccountId && m.ReceiverProfileId == otherUserAccountId) ||
                                               (m.ReceiverProfileId == userAccountId && m.SenderProfileId == otherUserAccountId)).ToList()
            .OrderByDescending(message => message.Timestamp);
    }

    public IEnumerable<Message> GetPreviewMessages(int userAccountId)
    {
        return Context.MessageTable.Where(message => message.SenderProfileId == userAccountId || message.ReceiverProfileId == userAccountId)
            .GroupBy(message => message.SenderProfileId == userAccountId ? message.ReceiverProfileId : message.SenderProfileId)
            .Select(group => group.OrderByDescending(message => message.Timestamp).First()).ToList();
    }

    public IEnumerable<Message> GetAllUnseenMessages(int userAccountId)
    {
        var messages = Context.MessageTable.Where(message => message.ReceiverProfileId == userAccountId && !message.IsSeen).ToList();
        return messages;
    }
}