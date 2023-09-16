using Commons.Communications.Authentication;
using Commons.Communications.Mcps;
using Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Repositories;
using RequestStatuses;
using Services;
using Services.Mcps;

namespace Test;

public class McpDataControllerTests
{
    [Test]
    public void AddNewMcp()
    {
        var mockUnitOfWork = new MockUnitOfWork();
        var mcpDataService = new McpDataService(mockUnitOfWork);

        var result = mcpDataService.AddNewMcp(new AddNewMcpRequest
        {
            Address = "test_address",
            Coordinate = default,
            Zone = null,
            Capacity = 100
        });

        Assert.IsInstanceOf<Success>(result.RequestStatus);
    }
    
    [Test]
    public void AddThenRemoveMcps()
    {
        var mockUnitOfWork = new MockUnitOfWork();
        var mcpDataService = new McpDataService(mockUnitOfWork);

        var addResult1 = mcpDataService.AddNewMcp(new AddNewMcpRequest
        {
            Address = "test_address",
            Coordinate = default,
            Zone = null,
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
            Coordinate = default,
            Zone = null,
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
        var mcpDataService = new McpDataService(mockUnitOfWork);

        var result = mcpDataService.RemoveMcp(new RemoveMcpRequest
        {
            McpId = 1
        });

        Assert.IsInstanceOf<DataEntryNotFound>(result.RequestStatus);
    }

    [Test]
    public void UpdateMcp()
    {
        var mockUnitOfWork = new MockUnitOfWork();
        var mcpDataService = new McpDataService(mockUnitOfWork);

        var result = mcpDataService.AddNewMcp(new AddNewMcpRequest
        {
            Address = "test_address",
            Coordinate = default,
            Zone = null,
            Capacity = 100
        });

        Assert.IsInstanceOf<Success>(result.RequestStatus);
        
        var newAddress = "test_address2";
        var updateResult = mcpDataService.UpdateMcp(new UpdateMcpRequest
        {
            McpId = 1,
            NewAddress = newAddress,
        });
        
        Assert.IsInstanceOf<Success>(updateResult.RequestStatus);
        Assert.AreEqual(newAddress, mockUnitOfWork.McpData.GetById(1).Address);
    }
}