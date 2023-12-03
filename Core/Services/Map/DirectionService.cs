using Commons.Communications.Map;
using Commons.RequestStatuses;
using Commons.Types;
using Newtonsoft.Json;
using Repositories.Managers;

namespace Services.Map;

public class DirectionService : IDirectionService
{
    private readonly IUnitOfWork _unitOfWork;

    public DirectionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public ParamRequestResult<GetDirectionResponse> GetDirection(GetDirectionRequest request)
    {
        var direction = RequestMapboxDirection(request.CurrentLocation, request.Destinations);
        return new ParamRequestResult<GetDirectionResponse>(new Success(), new GetDirectionResponse { Direction = direction });
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