using Commons.Communications.Map;
using Commons.RequestStatuses;
using Commons.Types;
using Newtonsoft.Json;
using Repositories.Managers;

namespace Services.Map;

public class DirectionService : IDirectionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocationService _locationService;

    public DirectionService(IUnitOfWork unitOfWork, ILocationService locationService)
    {
        _unitOfWork = unitOfWork;
        _locationService = locationService;
    }

    public ParamRequestResult<GetDirectionResponse> GetDirection(GetDirectionRequest request)
    {
        var mcpCoordinates = request.McpIds.Select(mcpId => _unitOfWork.McpDataRepository.GetById(mcpId).Coordinate).ToList();
        var mapboxDirection = RequestMapboxDirection(request.CurrentLocation, mcpCoordinates);
        return new ParamRequestResult<GetDirectionResponse>(new Success(),
            new GetDirectionResponse { Direction = new Direction(request.CurrentLocation, request.McpIds, mapboxDirection) });
    }

    public RawMapboxDirectionResponse GetRawDirection(Coordinate from, Coordinate to)
    {
        return RequestMapboxDirection(from, new List<Coordinate> { to });
    }

    private RawMapboxDirectionResponse RequestMapboxDirection(Coordinate fromLocation, List<Coordinate> toLocations)
    {
        var client = new HttpClient();
        var httpResponse = client.GetStringAsync(ConstructMapboxDirectionRequest(fromLocation, toLocations)).Result;
        var mapboxDirectionResponse = JsonConvert.DeserializeObject<RawMapboxDirectionResponse>(httpResponse);
        return mapboxDirectionResponse;
    }

    private string ConstructMapboxDirectionRequest(Coordinate currentLocation, List<Coordinate> destinationLocations)
    {
        return string.Format(Constants.MAPBOX_DIRECTION_API,
            currentLocation.ToStringApi(),
            String.Join(';', destinationLocations.Select(location => location.ToStringApi())));
    }
}