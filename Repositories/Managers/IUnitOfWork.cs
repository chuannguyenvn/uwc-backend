using Repositories.Implementations.Accounts;
using Repositories.Implementations.Mcps;
using Repositories.Implementations.Messages;
using Repositories.Implementations.Tasks;
using Repositories.Implementations.Vehicles;

namespace Repositories.Managers;

public interface IUnitOfWork
{
    IAccountRepository Accounts { get; }
    IMcpDataRepository McpData { get; }
    IMessageRepository Messages { get; }
    ITaskRepository TaskDatas { get; }
    IVehicleDataRepository VehicleData { get; }
    int Complete();
    Task<int> CompleteAsync();
}