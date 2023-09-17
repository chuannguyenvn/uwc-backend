﻿using Repositories.Implementations;
using Repositories.Implementations.Accounts;
using Repositories.Implementations.Mcps;
using Repositories.Implementations.Messages;
using Repositories.Implementations.UserProfiles;
using Repositories.Implementations.Vehicles;

namespace Repositories.Managers;

public interface IUnitOfWork 
{
    IAccountRepository Accounts { get; }
    IMcpDataRepository McpData { get; }
    VehicleDataRepository VehicleData { get; }
    IMessageRepository Messages { get; }
    UserProfileRepository UserProfiles { get; }
    int Complete();
    Task<int> CompleteAsync();
}