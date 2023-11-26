using Repositories.Managers;
using Commons.Communications.Mcps;
using Commons.HubHandlers;
using Commons.Models;
using Commons.RequestStatuses;
using Commons.Types;
using Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Services.Mcps;

public class McpDataService : IMcpDataService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHubContext<BaseHub> _hubContext;

    public McpDataService(IUnitOfWork unitOfWork, IHubContext<BaseHub> hubContext)
    {
        _unitOfWork = unitOfWork;
        _hubContext = hubContext;
    }

    public RequestResult AddNewMcp(AddNewMcpRequest request)
    {
        // TODO: Somehow check if the data entry already exist

        var mcpData = new McpData
        {
            Address = request.Address,
            Coordinate = request.Coordinate,
        };
        _unitOfWork.McpDataRepository.Add(mcpData);
        _unitOfWork.Complete();

        _hubContext.Clients.All.SendAsync(HubHandlers.McpLocation.BROADCAST_LOCATION, mcpData);

        return new RequestResult(new Success());
    }

    public RequestResult UpdateMcp(UpdateMcpRequest request)
    {
        if (!_unitOfWork.McpDataRepository.DoesIdExist(request.McpId)) return new RequestResult(new DataEntryNotFound());

        var mcpData = _unitOfWork.McpDataRepository.GetById(request.McpId);
        if (request.NewAddress != null) mcpData.Address = request.NewAddress;
        if (request.NewCoordinate != null) mcpData.Coordinate = request.NewCoordinate;
        _unitOfWork.Complete();

        _hubContext.Clients.All.SendAsync(HubHandlers.McpLocation.BROADCAST_LOCATION, mcpData);

        return new RequestResult(new Success());
    }

    public RequestResult RemoveMcp(RemoveMcpRequest request)
    {
        if (!_unitOfWork.McpDataRepository.DoesIdExist(request.McpId)) return new RequestResult(new DataEntryNotFound());

        var mcpData = _unitOfWork.McpDataRepository.GetById(request.McpId);
        _unitOfWork.McpDataRepository.Remove(mcpData);
        _unitOfWork.Complete();

        _hubContext.Clients.All.SendAsync(HubHandlers.McpLocation.BROADCAST_LOCATION, mcpData);

        return new RequestResult(new Success());
    }

    public ParamRequestResult<GetSingleMcpDataResponse> GetSingleMcpData(GetSingleMcpDataRequest request)
    {
        if (!_unitOfWork.McpDataRepository.DoesIdExist(request.McpId))
            return new ParamRequestResult<GetSingleMcpDataResponse>(new DataEntryNotFound());

        var result = _unitOfWork.McpDataRepository.GetSingle(request.McpId, request.HistoryCountLimit, request.HistoryDateTimeLimit);
        return new ParamRequestResult<GetSingleMcpDataResponse>(new Success(), new GetSingleMcpDataResponse() { Result = result });
    }

    public ParamRequestResult<GetMcpDataResponse> GetMcpData(McpDataQueryParameters parameters)
    {
        var result = _unitOfWork.McpDataRepository.GetData(parameters);
        return new ParamRequestResult<GetMcpDataResponse>(new Success(), new GetMcpDataResponse() { Results = result.ToList() });
    }

    public ParamRequestResult<GetEmptyRecordsResponse> GetEmptyRecords(GetEmptyRecordsRequest request)
    {
        if (!_unitOfWork.McpDataRepository.DoesIdExist(request.McpId))
            return new ParamRequestResult<GetEmptyRecordsResponse>(new DataEntryNotFound());

        var result = _unitOfWork.McpDataRepository.GetEmptyRecords(request.McpId, request.CountLimit, request.DateTimeLimit);
        return new ParamRequestResult<GetEmptyRecordsResponse>(new Success(), new GetEmptyRecordsResponse() { Results = result.ToList() });
    }

    public ParamRequestResult<GetFillLevelLogsResponse> GetFillLevelLogs(GetFillLevelLogsRequest request)
    {
        if (!_unitOfWork.McpDataRepository.DoesIdExist(request.McpId))
            return new ParamRequestResult<GetFillLevelLogsResponse>(new DataEntryNotFound());

        var result = _unitOfWork.McpDataRepository.GetFillLevelLogs(request.McpId, request.CountLimit, request.DateTimeLimit);
        return new ParamRequestResult<GetFillLevelLogsResponse>(new Success(), new GetFillLevelLogsResponse() { Results = result.ToList() });
    }
}