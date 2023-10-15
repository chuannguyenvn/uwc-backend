﻿using Repositories.Implementations.Accounts;
using Repositories.Implementations.Mcps;
using Repositories.Implementations.Messages;
using Repositories.Implementations.Vehicles;

namespace Repositories.Managers;

public interface IUnitOfWork 
{
    IAccountRepository Accounts { get; }
    IMcpDataRepository McpData { get; }
    IVehicleDataRepository VehicleData { get; }
    IMessageRepository Messages { get; }
    int Complete();
    Task<int> CompleteAsync();
}