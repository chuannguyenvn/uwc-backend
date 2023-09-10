using Commons.Categories;
using Commons.Communications.Mcps;
using Commons.Models;
using Commons.Types;
using Repositories;
using RequestStatuses;

namespace Services.Mcps;

public class McpDataService : IMcpDataService
{
    private readonly UnitOfWork _unitOfWork;

    public McpDataService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public RequestResult AddNewMcp(AddNewMcpRequest request)
    {
        var mcpData = new McpData
        {
            Address = request.Address,
            Coordinate = request.Coordinate,
            Zone = request.Zone,
            Capacity = request.Capacity
        };
        _unitOfWork.McpData.Add(mcpData);
        _unitOfWork.Complete();
        
        return new RequestResult(new Success());
    }

    public RequestResult GetAllStableData(McpQueryParameters parameters)
    {
        var result = _unitOfWork.McpData.GetData(parameters);
        return new RequestResult(new Success(), result);
    }

    public RequestResult GetAllVolatileData(McpQueryParameters parameters)
    {
        var result = _unitOfWork.McpData.GetData(parameters);
        return new RequestResult(new Success(), result);
    }
}