using Commons.Models;

namespace Repositories.Implementations;

public class AccountRepository : GenericRepository<Account>
{
    public AccountRepository(UwcDbContext context) : base(context)
    {
    }

    public bool DoesUsernameExist(string username)
    {
        return _context.Accounts.Any(a => a.Username == username);
    }

    public Account GetByUsername(string username)
    {
        return _context.Accounts.FirstOrDefault(a => a.Username == username);
    }
}