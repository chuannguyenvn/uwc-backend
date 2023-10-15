using Commons.Communications.Map;
using Commons.RequestStatuses;
using Commons.Types;
using Newtonsoft.Json;

namespace Services.Map;

public class DirectionService : IDirectionService
{
    private const string MAPBOX_DIRECTION_API =
        "https://api.mapbox.com/directions/v5/mapbox/driving-traffic/{0};{1}?geometries=geojson&access_token=pk.eyJ1IjoiY2h1YW4tbmd1eWVudm4iLCJhIjoiY2xsYTkycjJoMGg1MjNxbGhhcW5mMzNuOCJ9.tpAt14HVH_j1IKuKxsK31A";

    public ParamRequestResult<GetDirectionResponse> GetDirection(GetDirectionRequest request)
    {
        var direction = RequestMapboxDirection(request.CurrentLocation, request.Destinations);
        return new ParamRequestResult<GetDirectionResponse>(new Success(), new GetDirectionResponse { Direction = direction });
    }
    
    private RawMapboxDirectionResponse RequestMapboxDirection(Coordinate fromLocation, List<Coordinate> toLocations)
    {
        var client = new HttpClient();
        Console.WriteLine(ConstructMapboxDirectionRequest(fromLocation, toLocations));
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