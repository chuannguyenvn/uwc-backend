using Repositories.Generics;
using Repositories.Managers;
using Commons.Models;

namespace Repositories.Implementations.Messages;

public class MockMessageRepository: MockGenericRepository<Message>, IMessageRepository
{
    public MockMessageRepository(MockUwcDbContext mockUwcDbContext) : base(mockUwcDbContext)
    {
    }
    
    public IEnumerable<Message> GetMessagesBetweenTwoUsers(int senderAccountId, int receiverAccountId)
    {
        return Context.Messages.Where(m => (m.SenderAccountId == senderAccountId && m.ReceiverAccountId == receiverAccountId) ||
                                           (m.ReceiverAccountId == senderAccountId && m.SenderAccountId == receiverAccountId));
    }

    public IEnumerable<Message> GetPreviewMessages(int userAccountId)
    {
        return Context.Messages;
    }
}