using Commons.Models;

namespace Repositories.Implementations.Accounts;

public class MockAccountRepository : MockGenericRepository<Account>, IAccountRepository
{
    public MockAccountRepository(MockUwcDbContext mockUwcDbContext) : base(mockUwcDbContext)
    {
        for (int i = 0; i < 20; i++)
        {
            Add(new Account()
            {
                Username = "account_" + i,
                PasswordHash = "password",
                PasswordSalt = "",
            });
        }
    }

    public bool DoesUsernameExist(string username)
    {
        return Context.Accounts.Any(a => a.Username == username);
    }

    public Account GetByUsername(string username)
    {
        return Context.Accounts.FirstOrDefault(a => a.Username == username);
    }
}