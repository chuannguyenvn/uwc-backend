using Repositories.Implementations;
using Repositories.Implementations.Accounts;

namespace Repositories;

public interface IUnitOfWork 
{
    IAccountRepository Accounts { get; }
    McpDataRepository McpData { get; }
    VehicleDataRepository VehicleData { get; }
    MessageRepository Messages { get; }
    UserProfileRepository UserProfiles { get; }
    int Complete();
    Task<int> CompleteAsync();
}