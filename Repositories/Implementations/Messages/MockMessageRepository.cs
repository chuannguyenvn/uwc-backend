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
        return Context.Messages.Where(m => (m.SenderAccountID == senderAccountId && m.ReceiverAccountID == receiverAccountId) ||
                                           (m.ReceiverAccountID == senderAccountId && m.SenderAccountID == receiverAccountId));
    }

    public IEnumerable<Message> GetPreviewMessages(int userAccountId)
    {
        return Context.Messages;
    }
}