using System.Collections.Generic;

namespace Commons.Communications.Mcps
{
    public class McpFillLevelBroadcastData
    {
        /// <summary>
        /// Fill level is a float between 0 and 1.
        /// </summary>
        public Dictionary<int, float> FillLevelsById { get; set; }
    }
}