using Repositories.Implementations.Accounts;
using Repositories.Implementations.Mcps.McpData;
using Repositories.Implementations.Mcps.McpEmptyRecords;
using Repositories.Implementations.Mcps.McpFillLevelLogs;
using Repositories.Implementations.Messages;
using Repositories.Implementations.Settings;
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

        AccountRepository = new AccountRepository(_uwcDbContext);
        UserProfileRepository = new UserProfileRepository(_uwcDbContext);
        McpDataRepository = new McpDataRepository(_uwcDbContext);
        McpEmptyRecordRecordRepository = new McpEmptyRecordRepository(_uwcDbContext);
        McpFillLevelLogRepository = new McpFillLevelLogRepository(_uwcDbContext);
        MessageRepository = new MessageRepository(_uwcDbContext);
        TaskDataDataRepository = new TaskDataDataRepository(_uwcDbContext);
        VehicleDataRepository = new VehicleDataRepository(_uwcDbContext);
        SettingRepository = new SettingRepository(_uwcDbContext);
    }

    public IAccountRepository AccountRepository { get; }
    public IUserProfileRepository UserProfileRepository { get; }
    public IMcpDataRepository McpDataRepository { get; }
    public IMcpEmptyRecordRepository McpEmptyRecordRecordRepository { get; }
    public IMcpFillLevelLogRepository McpFillLevelLogRepository { get; }
    public ITaskDataRepository TaskDataDataRepository { get; }
    public IMessageRepository MessageRepository { get; }
    public IVehicleDataRepository VehicleDataRepository { get; }
    public ISettingRepository SettingRepository { get; }

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