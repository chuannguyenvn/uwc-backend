using System.Collections.Generic;
using System.Text.Json.Serialization;
using Commons.Types;

namespace Commons.Communications.Authentication
{
    public class InitializationData
    {
        [JsonInclude] public Dictionary<int, Coordinate> McpLocationByIds;

        [JsonConstructor]
        public InitializationData()
        {
        }
    }
}