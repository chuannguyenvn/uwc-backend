﻿using Repositories.Implementations.Accounts;
using Repositories.Implementations.Mcps;
using Repositories.Implementations.Mcps.McpData;
using Repositories.Implementations.Messages;
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
        Accounts = new MockAccountRepository(_uwcDbContext);
        UserProfiles = new MockUserProfileRepository(_uwcDbContext);
        McpData = new MockMcpDataRepository(_uwcDbContext);
        Messages = new MockMessageRepository(_uwcDbContext);
        TaskDatas = new MockTaskRepository(_uwcDbContext);
        VehicleData = new MockVehicleDataRepository(_uwcDbContext);
    }

    public IAccountRepository Accounts { get; }
    public IUserProfileRepository UserProfiles { get; }
    public IMcpDataRepository McpData { get; }
    public ITaskRepository TaskDatas { get; }
    public IMessageRepository Messages { get; }
    public IVehicleDataRepository VehicleData { get; }

    public int Complete()
    {
        return 0;
    }

    public Task<int> CompleteAsync()
    {
        return Task.FromResult(0);
    }
}