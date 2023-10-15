using Repositories.Implementations.Accounts;
using Repositories.Implementations.Mcps;
using Repositories.Implementations.Messages;
using Repositories.Implementations.Tasks;
using Repositories.Implementations.Vehicles;

namespace Repositories.Managers;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly UwcDbContext _uwcDbContext;

    public UnitOfWork(UwcDbContext uwcDbContext)
    {
        _uwcDbContext = uwcDbContext;

        Accounts = new AccountRepository(_uwcDbContext);
        McpData = new McpDataRepository(_uwcDbContext);
        Messages = new MessageRepository(_uwcDbContext);
        TaskDatas = new TaskRepository(_uwcDbContext);
        VehicleData = new VehicleDataRepository(_uwcDbContext);
    }

    public IAccountRepository Accounts { get; }
    public IMcpDataRepository McpData { get; }
    public ITaskRepository TaskDatas { get; }
    public IMessageRepository Messages { get; }
    public IVehicleDataRepository VehicleData { get; }

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