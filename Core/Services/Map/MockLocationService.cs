using Commons.Categories;
using Commons.Communications.Map;
using Commons.RequestStatuses;
using Commons.Types;
using Repositories.Managers;

namespace Services.Map;

public class MockLocationService : ILocationService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly Dictionary<int, Coordinate> _driverLocationsById = new();
    public Dictionary<int, Coordinate> DriverLocationsByAccountId => _driverLocationsById;

    private readonly Dictionary<int, Coordinate> _cleanerLocationsById = new();
    public Dictionary<int, Coordinate> CleanerLocationsByAccountId => _cleanerLocationsById;

    public MockLocationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public ParamRequestResult<GetLocationResponse> GetLocation(GetLocationRequest request)
    {
        if (_driverLocationsById.TryGetValue(request.AccountId, out var driverLocation))
            return new ParamRequestResult<GetLocationResponse>(new Success(), new GetLocationResponse()
            {
                Coordinate = driverLocation
            });

        if (_cleanerLocationsById.TryGetValue(request.AccountId, out var cleanerLocation))
            return new ParamRequestResult<GetLocationResponse>(new Success(), new GetLocationResponse()
            {
                Coordinate = cleanerLocation
            });

        return new ParamRequestResult<GetLocationResponse>(new DataEntryNotFound());
    }

    public RequestResult UpdateLocation(LocationUpdateRequest request)
    {
        var account = _unitOfWork.AccountRepository.GetById(request.AccountId);
        var userProfile = _unitOfWork.UserProfileRepository.GetById(account.UserProfileId);

        if (userProfile.UserRole == UserRole.Driver) _driverLocationsById[request.AccountId] = request.NewLocation;
        else if (userProfile.UserRole == UserRole.Cleaner) _cleanerLocationsById[request.AccountId] = request.NewLocation;

        return new RequestResult(new Success());
    }

    public void UpdateLocation(int accountId, Coordinate newLocation)
    {
        UpdateLocation(new LocationUpdateRequest
        {
            AccountId = accountId,
            NewLocation = newLocation
        });
    }
}