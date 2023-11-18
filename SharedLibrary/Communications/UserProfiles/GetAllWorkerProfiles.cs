using System.Collections.Generic;
using Commons.Models;

namespace Commons.Communications.UserProfiles
{
    public class GetAllWorkerProfilesResponse
    {
        public List<UserProfile> WorkerProfiles { get; set; }
    }
}