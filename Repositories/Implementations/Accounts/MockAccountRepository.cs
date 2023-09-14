using Commons.Models;

namespace Repositories.Implementations.Accounts;

public class MockAccountRepository : MockGenericRepository<Account>, IAccountRepository
{
    public MockAccountRepository(MockUwcDbContext mockUwcDbContext) : base(mockUwcDbContext)
    {
        Context.Accounts.Add(new Account()
        {
            Id = 1,
            Username = "admin",
            PasswordHash = "password",
            PasswordSalt = "salt",
            UserProfileID = 1
        });
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