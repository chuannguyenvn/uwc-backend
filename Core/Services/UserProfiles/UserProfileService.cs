using Commons.Categories;
using Commons.Communications.UserProfiles;
using Commons.Models;
using Commons.RequestStatuses;
using Repositories.Managers;

namespace Services.UserProfiles;

public class UserProfileService : IUserProfileService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserProfileService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public ParamRequestResult<UserProfile> GetUserProfileById(int id)
    {
        if (!_unitOfWork.UserProfileRepository.DoesIdExist(id)) return new ParamRequestResult<UserProfile>(new DataEntryNotFound());

        var userProfile = _unitOfWork.UserProfileRepository.GetById(id);
        return new ParamRequestResult<UserProfile>(new Success(), userProfile);
    }

    public ParamRequestResult<GetAllUserProfilesResponse> GetAllUserProfiles()
    {
        var userProfiles = _unitOfWork.UserProfileRepository.GetAll();
        return new ParamRequestResult<GetAllUserProfilesResponse>(new Success(),
            new GetAllUserProfilesResponse() { UserProfiles = userProfiles.ToList() });
    }

    public ParamRequestResult<GetAllWorkerProfilesResponse> GetAllWorkerProfiles()
    {
        var driverProfiles = _unitOfWork.UserProfileRepository.GetByUserRole(UserRole.Driver);
        var cleanerProfiles = _unitOfWork.UserProfileRepository.GetByUserRole(UserRole.Cleaner);
        var workerProfiles = driverProfiles.Concat(cleanerProfiles);
        return new ParamRequestResult<GetAllWorkerProfilesResponse>(new Success(),
            new GetAllWorkerProfilesResponse() { WorkerProfiles = workerProfiles.ToList() });
    }

    public ParamRequestResult<GetAllDriverProfilesResponse> GetAllDriverProfiles()
    {
        var driverProfiles = _unitOfWork.UserProfileRepository.GetByUserRole(UserRole.Driver);
        return new ParamRequestResult<GetAllDriverProfilesResponse>(new Success(),
            new GetAllDriverProfilesResponse() { DriverProfiles = driverProfiles.ToList() });
    }

    public ParamRequestResult<GetAllCleanerProfilesResponse> GetAllCleanerProfiles()
    {
        var cleanerProfiles = _unitOfWork.UserProfileRepository.GetByUserRole(UserRole.Cleaner);
        return new ParamRequestResult<GetAllCleanerProfilesResponse>(new Success(),
            new GetAllCleanerProfilesResponse() { CleanerProfiles = cleanerProfiles.ToList() });
    }
}