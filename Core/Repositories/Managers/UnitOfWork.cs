using Repositories.Implementations.Accounts;
using Repositories.Implementations.Mcps;
using Repositories.Implementations.Mcps.McpData;
using Repositories.Implementations.Mcps.McpEmptyRecords;
using Repositories.Implementations.Mcps.McpFillLevelLogs;
using Repositories.Implementations.Messages;
using Repositories.Implementations.Tasks;
using Repositories.Implementations.UserProfiles;
using Repositories.Implementations.Vehicles;

namespace Repositories.Managers;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly UwcDbContext _uwcDbContext;

    public UnitOfWork(UwcDbContext uwcDbContext)
    {
        _uwcDbContext = uwcDbContext;

        Accounts = new AccountRepository(_uwcDbContext);
        UserProfiles = new UserProfileRepository(_uwcDbContext);
        McpData = new McpDataRepository(_uwcDbContext);
        McpEmptyRecords = new McpEmptyRecordRepository(_uwcDbContext);
        McpFillLevelLogs = new McpFillLevelLogRepository(_uwcDbContext);
        Messages = new MessageRepository(_uwcDbContext);
        TaskDatas = new TaskRepository(_uwcDbContext);
        VehicleData = new VehicleDataRepository(_uwcDbContext);
    }

    public IAccountRepository Accounts { get; }
    public IUserProfileRepository UserProfiles { get; }
    public IMcpDataRepository McpData { get; }
    public IMcpEmptyRecordRepository McpEmptyRecords { get; }
    public IMcpFillLevelLogRepository McpFillLevelLogs { get; }
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