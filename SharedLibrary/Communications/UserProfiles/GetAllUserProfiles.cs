using System.Collections.Generic;
using Commons.Models;

namespace Commons.Communications.UserProfiles
{
    public class GetAllUserProfilesResponse
    {
        public List<UserProfile> UserProfiles { get; set; }
    }
}