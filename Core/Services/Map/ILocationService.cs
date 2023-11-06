using Commons.Communications.Map;
using Commons.Types;

namespace Services.Map;

public interface ILocationService : IHostedService, IDisposable
{
    public Dictionary<int, Coordinate> DriverLocationsByAccountId { get; }
    public Dictionary<int, Coordinate> CleanerLocationsByAccountId { get; }
    public ParamRequestResult<GetLocationResponse> GetLocation(GetLocationRequest request);
    public RequestResult UpdateLocation(LocationUpdateRequest request);
}