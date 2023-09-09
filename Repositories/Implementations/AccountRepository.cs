using Commons.Models;

namespace Repositories.Implementations;

public class AccountRepository : GenericRepository<Account>
{
    public AccountRepository(UwcDbContext context) : base(context)
    {
    }
}