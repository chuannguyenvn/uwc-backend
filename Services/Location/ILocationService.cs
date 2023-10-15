using Commons.Communications.Location;

namespace Services.Location;

public interface ILocationService : IHostedService, IDisposable
{
    public RequestResult UpdateLocation(int accountId, LocationUpdateRequest request);
}