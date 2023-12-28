using Repositories.Generics;
using Commons.Models;

namespace Repositories.Implementations.Messages;

public interface IMessageRepository : IGenericRepository<Message>
{
    public IEnumerable<Message> GetMessagesBetweenTwoUsers(int userAccountId, int otherUserAccountId);
    public IEnumerable<Message> GetPreviewMessages(int userAccountId);
    public IEnumerable<Message> GetAllUnseenMessages(int userAccountId);
}