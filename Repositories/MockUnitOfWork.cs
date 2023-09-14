using Repositories.Implementations;
using Repositories.Implementations.Accounts;

namespace Repositories;

public class MockUnitOfWork : IUnitOfWork
{
    private readonly MockUwcDbContext _uwcDbContext;

    public MockUnitOfWork()
    {
        _uwcDbContext = new MockUwcDbContext();
        Accounts = new MockAccountRepository(_uwcDbContext);
    }

    public IAccountRepository Accounts { get; }
    public McpDataRepository McpData { get; }
    public VehicleDataRepository VehicleData { get; }
    public MessageRepository Messages { get; }
    public UserProfileRepository UserProfiles { get; }

    public int Complete()
    {
        return 0;
    }

    public Task<int> CompleteAsync()
    {
        return Task.FromResult(0);
    }
}