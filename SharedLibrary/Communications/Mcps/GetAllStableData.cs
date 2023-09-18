using System.Collections.Generic;
using Commons.Models;

namespace Commons.Communications.Mcps
{
    public class GetAllStableDataResponse
    {
        public List<McpData> Results { get; set; }
    }
}