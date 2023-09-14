using Commons.Communications.Mcps;
using Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RequestStatuses;
using Services;
using Services.Mcps;

namespace Test;

public class McpDataControllerTests
{
    private McpDataController _mcpDataController;
    private Mock<IMcpDataService> _mcpDataServiceMock;

    private List<Mock<AddNewMcpRequest>> _addNewMcpRequestMocks = new();

    [SetUp]
    public void Setup()
    {
        _mcpDataServiceMock = new Mock<IMcpDataService>();
        _mcpDataController = new McpDataController(_mcpDataServiceMock.Object);

        for (int i = 0; i < 20; i++) _addNewMcpRequestMocks.Add(new Mock<AddNewMcpRequest>());

        _addNewMcpRequestMocks[0].Object.Capacity = 100;
        _addNewMcpRequestMocks[1].Object.Capacity = 100;
        _addNewMcpRequestMocks[2].Object.Capacity = 100;
    }

    [Test]
    public void GetAllStableData()
    {
        // _mcpDataServiceMock.Setup(service => service.GetAllStableData(new McpQueryParameters()
        //     {
        //         Filter = new()
        //         {
        //             CapacityRange = new Range<float>(99, 101),
        //         }
        //     }))
        //     .Returns();
        //
        // var result = _mcpDataController.GetAllStableData(new McpQueryParameters());
        //
        // Assert.IsInstanceOf<OkResult>(result);
    }

    [Test]
    public void RemoveNonExistingMcp()
    {
        var mock = new Mock<IMcpDataService>();
        mock.Setup(service => service.RemoveMcp(It.Is<RemoveMcpRequest>(request => request.McpId == It.IsAny<int>())))
            .Returns(It.Is<RequestResult>(result=> result.RequestStatus.StatusType == HttpResponseStatusType.BadRequest));
        mock.SetupAllProperties();

        var controller = new McpDataController(mock.Object);

        var result = controller.RemoveMcp(new RemoveMcpRequest { McpId = 10000000 });

        Assert.IsInstanceOf<NotFoundResult>(result);
    }
}