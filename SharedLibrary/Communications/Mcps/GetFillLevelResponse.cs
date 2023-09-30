using System.Collections.Generic;

namespace Commons.Communications.Mcps
{
    public class GetFillLevelResponse
    {
        public Dictionary<int, float> FillLevelsById { get; set; }
    }
}