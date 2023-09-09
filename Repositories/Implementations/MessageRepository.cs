using Commons.Models;

namespace Repositories.Implementations;

public class MessageRepository : GenericRepository<Message>
{
    public MessageRepository(UwcDbContext context) : base(context)
    {
    }
}