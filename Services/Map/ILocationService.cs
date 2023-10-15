using Commons.Communications.Map;

namespace Services.Map;

public interface ILocationService : IHostedService, IDisposable
{
    public RequestResult UpdateLocation(LocationUpdateRequest request);
}