using Repositories.Generics;
using Repositories.Managers;
using Commons.Models;

namespace Repositories.Implementations.Messages;

public class MessageRepository : GenericRepository<Message>, IMessageRepository
{
    public MessageRepository(UwcDbContext context) : base(context)
    {
    }

    public IEnumerable<Message> GetMessagesBetweenTwoUsers(int userAccountId, int otherUserAccountId)
    {
        return Context.MessageTable.Where(m => (m.SenderAccountId == userAccountId && m.ReceiverAccountId == otherUserAccountId) ||
                                               (m.ReceiverAccountId == userAccountId && m.SenderAccountId == otherUserAccountId)).ToList()
            .OrderByDescending(message => message.Timestamp);
    }

    public IEnumerable<Message> GetPreviewMessages(int userAccountId)
    {
        return Context.MessageTable.Where(message => message.SenderAccountId == userAccountId || message.ReceiverAccountId == userAccountId)
            .GroupBy(message => message.SenderAccountId == userAccountId ? message.ReceiverAccountId : message.SenderAccountId)
            .Select(group => group.OrderByDescending(message => message.Timestamp).First()).ToList();
    }
}