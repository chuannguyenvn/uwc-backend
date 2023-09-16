using Commons.Models;

namespace Repositories.Implementations.Messages;

public interface IMessageRepository : IGenericRepository<Message>
{
    public IEnumerable<Message> GetMessagesBetweenTwoUsers(int senderAccountId, int receiverAccountId);
    public IEnumerable<Message> GetPreviewMessages(int userAccountId);
}