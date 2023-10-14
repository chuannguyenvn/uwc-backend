using Repositories.Generics;
using Repositories.Managers;
using Commons.Models;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Implementations.Messages;

public class MessageRepository : GenericRepository<Message>, IMessageRepository
{
    public MessageRepository(UwcDbContext context) : base(context)
    {
    }

    public IEnumerable<Message> GetMessagesBetweenTwoUsers(int userAccountId, int otherUserAccountId)
    {
        return Context.Messages.Where(m => (m.SenderAccountId == userAccountId && m.ReceiverAccountId == otherUserAccountId) ||
                                           (m.ReceiverAccountId == userAccountId && m.SenderAccountId == otherUserAccountId));
    }

    public IEnumerable<Message> GetPreviewMessages(int userAccountId)
    {
        return Context.Messages.Where(message => message.SenderAccountId == userAccountId || message.ReceiverAccountId == userAccountId)
            .GroupBy(message => message.SenderAccountId == userAccountId ? message.ReceiverAccountId : message.SenderAccountId)
            .Select(group => group.OrderByDescending(message => message.Timestamp).First()).ToList();
    }
}