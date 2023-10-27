using System.Collections.Generic;

namespace Commons.Communications.Mcps
{
    public class GetFillLevelRequest
    {
        public List<int> McpIds { get; set; }
    }

    public class GetFillLevelResponse
    {
        public Dictionary<int, float> FillLevelsById { get; set; }
    }
}