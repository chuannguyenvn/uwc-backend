using Commons.Communications.UserProfiles;
using Commons.Models;

namespace Services.UserProfiles;

public interface IUserProfileService
{
    public ParamRequestResult<UserProfile> GetUserProfileById(int id);
    public ParamRequestResult<GetAllUserProfilesResponse> GetAllUserProfiles();
    public ParamRequestResult<GetAllDriverProfilesResponse> GetAllDriverProfiles();
    public ParamRequestResult<GetAllCleanerProfilesResponse> GetAllCleanerProfiles();
}