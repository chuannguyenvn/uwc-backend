using Repositories.Implementations;
using Repositories.Implementations.Accounts;
using Repositories.Implementations.Mcps;
using Repositories.Implementations.Messages;

namespace Repositories.Managers;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly UwcDbContext _uwcDbContext;

    public UnitOfWork(UwcDbContext uwcDbContext)
    {
        _uwcDbContext = uwcDbContext;

        Accounts = new AccountRepository(_uwcDbContext);
        McpData = new McpDataRepository(_uwcDbContext);
        VehicleData = new VehicleDataRepository(_uwcDbContext);
        Messages = new MessageRepository(_uwcDbContext);
        UserProfiles = new UserProfileRepository(_uwcDbContext);
    }

    public IAccountRepository Accounts { get; }
    public IMcpDataRepository McpData { get; }
    public VehicleDataRepository VehicleData { get; }
    public IMessageRepository Messages { get; }
    public UserProfileRepository UserProfiles { get; }

    public void Dispose()
    {
        _uwcDbContext.Dispose();
    }

    public int Complete()
    {
        return _uwcDbContext.SaveChanges();
    }

    public Task<int> CompleteAsync()
    {
        return _uwcDbContext.SaveChangesAsync();
    }
}