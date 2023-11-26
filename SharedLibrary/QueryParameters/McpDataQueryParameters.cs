using System;
using System.Collections.Generic;
using System.Linq;
using Commons.Categories;
using Commons.Models;

namespace Commons.Types
{
    public class McpDataQueryParameters : QueryParameters<McpData>
    {
        public FilterBy Filter { get; } = new FilterBy();
        public Dictionary<SortBy, SortStrategy> Sort { get; } = new Dictionary<SortBy, SortStrategy>();

        public enum SortBy
        {
            Address,
            CurrentLoadPercentage,
        }

        public class FilterBy
        {
            public Range<float>? CapacityRange { get; set; } = null;
            public Range<float>? CurrentLoadRange { get; set; } = null;
            public Range<float>? CurrentLoadPercentageRange { get; set; } = null;
        }

        public override void ExecuteFilter(ref IEnumerable<McpData> data)
        {
   
        }

        public override void ExecuteSort(ref IEnumerable<McpData> data)
        {
            foreach (var (sortBy, strategy) in Sort)
            {
                switch (sortBy)
                {
                    case SortBy.Address:
                        if (strategy == SortStrategy.Ascending)
                            data = data.OrderBy(mcp => mcp.Address);
                        else
                            data = data.OrderByDescending(mcp => mcp.Address);
                        break;
                    case SortBy.CurrentLoadPercentage:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public override void ExecutePaginate(ref IEnumerable<McpData> data)
        {
            
        }
    }
}