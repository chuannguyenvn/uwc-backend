using System.Collections.Generic;

namespace Commons.Communications.Mcps
{
    public class GetFillLevelRequest
    {
        public List<int> McpIds { get; set; }
    }

    public class GetFillLevelResponse
    {
        /// <summary>
        /// Fill level is a float between 0 and 1.
        /// </summary>
        public Dictionary<int, float> FillLevelsById { get; set; }
    }
}