using Repositories.Implementations;

namespace Repositories;

public class UnitOfWork : IDisposable
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

    public AccountRepository Accounts { get; }
    public McpDataRepository McpData { get; }
    public VehicleDataRepository VehicleData { get; }
    public MessageRepository Messages { get; }
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