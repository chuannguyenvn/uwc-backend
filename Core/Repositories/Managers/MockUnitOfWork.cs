using Helpers;
using Repositories.Implementations.Accounts;
using Repositories.Implementations.Mcps;
using Repositories.Implementations.Mcps.McpData;
using Repositories.Implementations.Mcps.McpEmptyRecords;
using Repositories.Implementations.Mcps.McpFillLevelLogs;
using Repositories.Implementations.Messages;
using Repositories.Implementations.Settings;
using Repositories.Implementations.Tasks;
using Repositories.Implementations.UserProfiles;
using Repositories.Implementations.Vehicles;

namespace Repositories.Managers;

public class MockUnitOfWork : IUnitOfWork
{
    private readonly MockUwcDbContext _uwcDbContext;

    public MockUnitOfWork()
    {
        _uwcDbContext = new MockUwcDbContext();
        AccountRepository = new MockAccountRepository(_uwcDbContext);
        UserProfileRepository = new MockUserProfileRepository(_uwcDbContext);
        McpDataRepository = new MockMcpDataRepository(_uwcDbContext);
        McpEmptyRecordRecordRepository = new MockMcpEmptyRecordRepository(_uwcDbContext);
        McpFillLevelLogRepository = new MockMcpFillLevelLogRepository(_uwcDbContext);
        MessageRepository = new MockMessageRepository(_uwcDbContext);
        TaskDataDataRepository = new MockTaskDataRepository(_uwcDbContext);
        VehicleDataRepository = new MockVehicleDataRepository(_uwcDbContext);
        SettingRepository = new MockSettingRepository(_uwcDbContext);

        var dataSeeder = new DatabaseSeeder(this);
        dataSeeder.SeedDatabase();
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

    public int Complete()
    {
        return 0;
    }

    public Task<int> CompleteAsync()
    {
        return Task.FromResult(0);
    }
}