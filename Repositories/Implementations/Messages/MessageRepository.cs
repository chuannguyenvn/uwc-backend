using Repositories.Generics;
using Repositories.Managers;
using Commons.Models;

namespace Repositories.Implementations.Messages;

public class MessageRepository : GenericRepository<Message>, IMessageRepository
{
    public MessageRepository(UwcDbContext context) : base(context)
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