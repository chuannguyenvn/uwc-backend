using Commons.Models;

namespace Repositories.Implementations.Messages;

public class MessageRepository : GenericRepository<Message>, IMessageRepository
{
    public MessageRepository(UwcDbContext context) : base(context)
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