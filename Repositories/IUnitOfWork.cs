using Repositories.Implementations;
using Repositories.Implementations.Accounts;
using Repositories.Implementations.Mcps;

namespace Repositories;

public interface IUnitOfWork 
{
    IAccountRepository Accounts { get; }
    IMcpDataRepository McpData { get; }
    VehicleDataRepository VehicleData { get; }
    MessageRepository Messages { get; }
    UserProfileRepository UserProfiles { get; }
    int Complete();
    Task<int> CompleteAsync();
}