using Repositories.Managers;
using Services.Mcps;
using Commons.Communications.Mcps;
using Commons.RequestStatuses;
using Commons.Types;
using Hubs;
using Microsoft.AspNetCore.SignalR;
using Moq;

namespace Test;

public class McpDataServiceTests
{
    private Mock<IHubContext<BaseHub>> _mockBaseHub = new Mock<IHubContext<BaseHub>>();

    [SetUp]
    public void Setup()
    {
        _mockBaseHub.Setup(mock => mock.Clients.All.SendCoreAsync(It.IsAny<string>(), It.IsAny<object[]>(), default));
    }

    [Test]
    public void AddNewMcp()
    {
        var mockUnitOfWork = new MockUnitOfWork();
        var mcpDataService = new McpDataService(mockUnitOfWork, _mockBaseHub.Object);

        var result = mcpDataService.AddNewMcp(new AddNewMcpRequest
        {
            Address = "test_address",
            Coordinate = new Coordinate(),
            Capacity = 100
        });

        Assert.IsInstanceOf<Success>(result.RequestStatus);
    }

    [Test]
    public void AddThenRemoveMcps()
    {
        var mockUnitOfWork = new MockUnitOfWork();
        var mcpDataService = new McpDataService(mockUnitOfWork, _mockBaseHub.Object);

        var addResult1 = mcpDataService.AddNewMcp(new AddNewMcpRequest
        {
            Address = "test_address",
            Coordinate = new Coordinate(),
            Capacity = 100
        });

        Assert.IsInstanceOf<Success>(addResult1.RequestStatus);

        var removeResult1 = mcpDataService.RemoveMcp(new RemoveMcpRequest
        {
            McpId = 1
        });

        Assert.IsInstanceOf<Success>(removeResult1.RequestStatus);

        var addResult2 = mcpDataService.AddNewMcp(new AddNewMcpRequest
        {
            Address = "test_address2",
            Coordinate = new Coordinate(),
            Capacity = 100
        });

        Assert.IsInstanceOf<Success>(addResult2.RequestStatus);

        var removeResult2 = mcpDataService.RemoveMcp(new RemoveMcpRequest
        {
            McpId = 2
        });

        Assert.IsInstanceOf<Success>(removeResult2.RequestStatus);
    }

    [Test]
    public void RemoveNonExistentMcp()
    {
        var mockUnitOfWork = new MockUnitOfWork();
        var mcpDataService = new McpDataService(mockUnitOfWork, _mockBaseHub.Object);

        var result = mcpDataService.RemoveMcp(new RemoveMcpRequest
        {
            McpId = 10000000
        });

        Assert.IsInstanceOf<DataEntryNotFound>(result.RequestStatus);
    }

    [Test]
    public void UpdateMcp()
    {
        var mockUnitOfWork = new MockUnitOfWork();
        var mcpDataService = new McpDataService(mockUnitOfWork, _mockBaseHub.Object);

        var result = mcpDataService.AddNewMcp(new AddNewMcpRequest
        {
            Address = "test_address",
            Coordinate = new Coordinate(),
            Capacity = 100
        });

        Assert.IsInstanceOf<Success>(result.RequestStatus);

        var newAddress = "test_address2";
        var updateResult = mcpDataService.UpdateMcp(new UpdateMcpRequest
        {
            McpId = 1,
            NewAddress = newAddress,
            NewCoordinate = null,
        });

        Assert.IsInstanceOf<Success>(updateResult.RequestStatus);
        Assert.AreEqual(newAddress, mockUnitOfWork.McpDataRepository.GetById(1).Address);
    }
}