using Repositories.Managers;
using Commons.Communications.Mcps;
using Commons.Models;
using Commons.RequestStatuses;
using Commons.Types;

namespace Services.Mcps;

public class McpDataService : IMcpDataService
{
    private readonly IUnitOfWork _unitOfWork;

    public McpDataService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public RequestResult AddNewMcp(AddNewMcpRequest request)
    {
        // TODO: Somehow check if the data entry already exist

        var mcpData = new McpData
        {
            Address = request.Address,
            Coordinate = request.Coordinate,
            Zone = request.Zone,
            Capacity = request.Capacity // TODO: Range check this
        };
        _unitOfWork.McpData.Add(mcpData);
        _unitOfWork.Complete();

        return new RequestResult(new Success());
    }

    public RequestResult UpdateMcp(UpdateMcpRequest request)
    {
        if (!_unitOfWork.McpData.DoesIdExist(request.McpId)) return new RequestResult(new DataEntryNotFound());

        var mcpData = _unitOfWork.McpData.GetById(request.McpId);
        if (request.NewAddress != null) mcpData.Address = request.NewAddress;
        if (request.NewCoordinate != null) mcpData.Coordinate = request.NewCoordinate.Value;
        if (request.NewZone != null) mcpData.Zone = request.NewZone;
        if (request.NewCapacity != null) mcpData.Capacity = request.NewCapacity.Value;
        _unitOfWork.Complete();

        return new RequestResult(new Success());
    }

    public RequestResult RemoveMcp(RemoveMcpRequest request)
    {
        if (!_unitOfWork.McpData.DoesIdExist(request.McpId)) return new RequestResult(new DataEntryNotFound());

        var mcpData = _unitOfWork.McpData.GetById(request.McpId);
        _unitOfWork.McpData.Remove(mcpData);
        _unitOfWork.Complete();

        return new RequestResult(new Success());
    }

    public ParamRequestResult<GetAllStableDataResponse> GetAllStableData(McpQueryParameters parameters)
    {
        var result = _unitOfWork.McpData.GetData(parameters);
        return new ParamRequestResult<GetAllStableDataResponse>(new Success(), new GetAllStableDataResponse() { Results = result.ToList() });
    }

    public RequestResult GetAllVolatileData(McpQueryParameters parameters)
    {
        var result = _unitOfWork.McpData.GetData(parameters);
        return new ParamRequestResult<GetAllVolatileDataResponse>(new Success(), new GetAllVolatileDataResponse() { Results = result.ToList() });
    }
}