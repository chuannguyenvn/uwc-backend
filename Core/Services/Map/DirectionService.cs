using Commons.Communications.Map;
using Commons.RequestStatuses;
using Commons.Types;
using Newtonsoft.Json;
using Repositories.Managers;

namespace Services.Map;

public class DirectionService : IDirectionService
{
    private const string MAPBOX_DIRECTION_API =
        "https://api.mapbox.com/directions/v5/mapbox/driving-traffic/{0};{1}?geometries=geojson&steps=true&access_token=pk.eyJ1IjoiY2h1YW4tbmd1eWVudm4iLCJhIjoiY2xsYTkycjJoMGg1MjNxbGhhcW5mMzNuOCJ9.tpAt14HVH_j1IKuKxsK31A";

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
        return string.Format(MAPBOX_DIRECTION_API,
            currentLocation.ToStringApi(),
            String.Join(';', destinationLocations.Select(location => location.ToStringApi())));
    }
}