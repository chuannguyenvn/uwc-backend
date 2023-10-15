using Commons.Communications.Map;
using Commons.Types;

namespace Services.Map;

public interface ILocationService : IHostedService, IDisposable
{
    public Dictionary<int, Coordinate> LocationsByAccountId { get; }
    public RequestResult UpdateLocation(LocationUpdateRequest request);
}