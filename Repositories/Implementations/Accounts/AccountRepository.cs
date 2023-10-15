using Commons.Categories;
using Repositories.Generics;
using Repositories.Managers;
using Commons.Models;

namespace Repositories.Implementations.Accounts;

public class AccountRepository : GenericRepository<Account>, IAccountRepository
{
    public AccountRepository(UwcDbContext context) : base(context)
    {
    }

    public bool DoesUsernameExist(string username)
    {
        return Context.Accounts.Any(a => a.Username == username);
    }

    public Account GetByUsername(string username)
    {
        return Context.Accounts.FirstOrDefault(a => a.Username == username);
    }

    public IEnumerable<Account> GetByUserRole(UserRole userRole)
    {
        return Context.Accounts.Where(a => a.UserRole == userRole);
    }
}