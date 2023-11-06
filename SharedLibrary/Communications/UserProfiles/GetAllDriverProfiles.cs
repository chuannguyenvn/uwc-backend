using System.Collections.Generic;
using Commons.Models;

namespace Commons.Communications.UserProfiles
{
    public class GetAllDriverProfilesResponse
    {
        public List<UserProfile> DriverProfiles { get; set; }
    }
}