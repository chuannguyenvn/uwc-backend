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

public interface IUnitOfWork
{
    IAccountRepository AccountRepository { get; }
    IUserProfileRepository UserProfileRepository { get; }
    IMcpDataRepository McpDataRepository { get; }
    IMcpEmptyRecordRepository McpEmptyRecordRecordRepository { get; }
    IMcpFillLevelLogRepository McpFillLevelLogRepository { get; }
    IMessageRepository MessageRepository { get; }
    ITaskDataRepository TaskDataDataRepository { get; }
    IVehicleDataRepository VehicleDataRepository { get; }
    ISettingRepository SettingRepository { get; }
    int Complete();
    Task<int> CompleteAsync();
}