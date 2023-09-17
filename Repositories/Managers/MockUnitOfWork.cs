using Repositories.Implementations;
using Repositories.Implementations.Accounts;
using Repositories.Implementations.Mcps;
using Repositories.Implementations.Messages;
using Repositories.Implementations.UserProfiles;
using Repositories.Implementations.Vehicles;

namespace Repositories.Managers;

public class MockUnitOfWork : IUnitOfWork
{
    private readonly MockUwcDbContext _uwcDbContext;

    public MockUnitOfWork()
    {
        _uwcDbContext = new MockUwcDbContext();
        Accounts = new MockAccountRepository(_uwcDbContext);
        McpData = new MockMcpDataRepository(_uwcDbContext);
        Messages = new MockMessageRepository(_uwcDbContext);
    }

    public IAccountRepository Accounts { get; }
    public IMcpDataRepository McpData { get; }
    public VehicleDataRepository VehicleData { get; }
    public IMessageRepository Messages { get; }
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