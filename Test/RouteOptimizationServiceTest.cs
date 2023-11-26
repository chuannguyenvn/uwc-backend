using Commons.Models;
using Commons.Types;
using Hubs;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Repositories.Managers;
using Services.Map;
using Services.Mcps;

namespace Test;

public class RouteOptimizationServiceTest
{
    private readonly Mock<IHubContext<BaseHub>> _mockBaseHub = new();

    [SetUp]
    public void Setup()
    {
        _mockBaseHub.Setup(mock => mock.Clients.Client(It.IsAny<string>()).SendCoreAsync(It.IsAny<string>(), It.IsAny<object[]>(), default));
    }

    [Test]
    public void Test_FillLevelPriority()
    {
        // The distance between the worker and mcp 1 is equal to the distance between the worker and mcp 2
        // The deadlines of the tasks are equal
        // The fill level of mcp 2 is higher than the fill level of mcp 1
        // => The worker should go to mcp 2 first

        #region Arrange

        var mockUnitOfWork = new MockUnitOfWork();
        var mockMcpFillLevelService = new MockMcpFillLevelService(mockUnitOfWork);
        var mockLocationService = new MockLocationService(mockUnitOfWork);
        var routeOptimizationService = new RouteOptimizationService(mockUnitOfWork, mockLocationService, mockMcpFillLevelService);

        // The supervisor's ID
        var supervisorId = 1;

        // The worker's ID
        var workerId = 11;

        // The mcps' IDs
        var mcp1Id = 1;
        var mcp2Id = 2;

        // Start with a clean slate (no pre-made tasks)
        mockUnitOfWork.TaskDataRepository.RemoveAll();

        // Set the worker's location
        mockLocationService.UpdateLocation(workerId, new Coordinate(10.5, 100));

        // Set the mcp locations
        mockUnitOfWork.McpDataRepository.GetById(mcp1Id).Coordinate = new Coordinate(10, 100);
        mockUnitOfWork.McpDataRepository.GetById(mcp2Id).Coordinate = new Coordinate(11, 100);

        // Set the mcp fill levels
        mockMcpFillLevelService.SetFillLevel(mcp1Id, 0.1f);
        mockMcpFillLevelService.SetFillLevel(mcp2Id, 0.9f);

        // Task with a nearly full mcp
        var task1 = new TaskData
        {
            AssignerId = supervisorId,
            AssigneeId = workerId,
            McpDataId = mcp1Id,
            CompleteByTimestamp = DateTime.Now.AddHours(1), // Same deadline
        };
        mockUnitOfWork.TaskDataRepository.Add(task1);

        // Task with an empty mcp
        var task2 = new TaskData
        {
            AssignerId = supervisorId,
            AssigneeId = workerId,
            McpDataId = mcp2Id,
            CompleteByTimestamp = DateTime.Now.AddHours(1), // Same deadline
        };
        mockUnitOfWork.TaskDataRepository.Add(task2);

        #endregion


        #region Act

        var optimizedTasks = routeOptimizationService.OptimizeRoute(mockUnitOfWork.UserProfileRepository.GetById(workerId));

        #endregion


        #region Assert

        Assert.That(optimizedTasks[0].McpDataId, Is.EqualTo(mcp2Id)); // First task must be mcp2
        Assert.That(optimizedTasks[1].McpDataId, Is.EqualTo(mcp1Id)); // Second task must be mcp1

        #endregion
    }

    [Test]
    public void Test_DeadlinePriority()
    {
        // The distance between the worker and mcp 1 is equal to the distance between the worker and mcp 2
        // The fill level of mcp 1 is equal to the fill level of mcp 2
        // The deadlines of mcp 2 is earlier than the deadline of mcp 1
        // => The worker should go to mcp 2 first

        #region Arrange

        var mockUnitOfWork = new MockUnitOfWork();
        var mockMcpFillLevelService = new MockMcpFillLevelService(mockUnitOfWork);
        var mockLocationService = new MockLocationService(mockUnitOfWork);
        var routeOptimizationService = new RouteOptimizationService(mockUnitOfWork, mockLocationService, mockMcpFillLevelService);

        // The supervisor's ID
        var supervisorId = 1;

        // The worker's ID
        var workerId = 11;

        // The mcps' IDs
        var mcp1Id = 1;
        var mcp2Id = 2;

        // Start with a clean slate (no pre-made tasks)
        mockUnitOfWork.TaskDataRepository.RemoveAll();

        // Set the worker's location
        mockLocationService.UpdateLocation(workerId, new Coordinate(10.5, 100));

        // Set the mcp locations
        mockUnitOfWork.McpDataRepository.GetById(mcp1Id).Coordinate = new Coordinate(10, 100);
        mockUnitOfWork.McpDataRepository.GetById(mcp2Id).Coordinate = new Coordinate(11, 100);

        // Set the mcp fill levels
        mockMcpFillLevelService.SetFillLevel(mcp1Id, 0.5f);
        mockMcpFillLevelService.SetFillLevel(mcp2Id, 0.5f);

        // Task with later deadline
        var task1 = new TaskData
        {
            AssignerId = supervisorId,
            AssigneeId = workerId,
            McpDataId = mcp1Id,
            CompleteByTimestamp = DateTime.Now.AddHours(10), // Later deadline
        };
        mockUnitOfWork.TaskDataRepository.Add(task1);

        // Task with earlier deadline
        var task2 = new TaskData
        {
            AssignerId = supervisorId,
            AssigneeId = workerId,
            McpDataId = mcp2Id,
            CompleteByTimestamp = DateTime.Now.AddHours(1), // Earlier deadline
        };
        mockUnitOfWork.TaskDataRepository.Add(task2);

        #endregion


        #region Act

        var optimizedTasks = routeOptimizationService.OptimizeRoute(mockUnitOfWork.UserProfileRepository.GetById(workerId));

        #endregion


        #region Assert

        Assert.That(optimizedTasks[0].McpDataId, Is.EqualTo(mcp2Id)); // First task must be mcp2
        Assert.That(optimizedTasks[1].McpDataId, Is.EqualTo(mcp1Id)); // Second task must be mcp1

        #endregion
    }
}