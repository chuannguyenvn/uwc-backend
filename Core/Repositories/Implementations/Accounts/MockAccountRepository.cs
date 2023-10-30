using Repositories.Generics;
using Repositories.Managers;
using Commons.Models;

namespace Repositories.Implementations.Accounts;

public class MockAccountRepository : MockGenericRepository<Account>, IAccountRepository
{
    public MockAccountRepository(MockUwcDbContext mockUwcDbContext) : base(mockUwcDbContext)
    {
    }

    public bool DoesUsernameExist(string username)
    {
        return Context.AccountTable.Any(a => a.Username == username);
    }

    public Account GetByUsername(string username)
    {
        return Context.AccountTable.FirstOrDefault(a => a.Username == username);
    }

    public override void Add(Account entity)
    {
        base.Add(entity);
        entity.UserProfile.AccountId = entity.Id;
        entity.UserProfile.Account = entity;
        entity.UserProfileId = entity.UserProfile.Id;
        Context.UserProfileTable.Add(entity.UserProfile);
    }
}