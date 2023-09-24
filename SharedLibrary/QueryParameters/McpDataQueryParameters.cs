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
            Capacity,
            CurrentLoad,
            CurrentLoadPercentage,
        }

        public class FilterBy
        {
            public Zone? Zone { get; set; } = null;
            public Range<float>? CapacityRange { get; set; } = null;
            public Range<float>? CurrentLoadRange { get; set; } = null;
            public Range<float>? CurrentLoadPercentageRange { get; set; } = null;
        }

        public override void ExecuteFilter(ref IEnumerable<McpData> data)
        {
            if (Filter.Zone != null)
            {
                data = data.Where(mcp => mcp.Zone == Filter.Zone);
            }

            if (Filter.CapacityRange != null)
            {
                data = data.Where(mcp => Filter.CapacityRange.Value.Contains(mcp.Capacity));
            }

            // if (parameters.Filter.CurrentLoadRange != null)
            // {
            //     data = data.Where(mcp => parameters.Filter.CurrentLoadRange.Value.Contains(mcp.CurrentLoad));
            // }

            // if (parameters.Filter.CurrentLoadPercentageRange != null)
            // {
            //     data = data.Where(mcp => parameters.Filter.CurrentLoadPercentageRange.Value.Contains(mcp.CurrentLoadPercentage));
            // }
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
                    case SortBy.Capacity:
                        if (strategy == SortStrategy.Ascending)
                            data = data.OrderBy(mcp => mcp.Capacity);
                        else
                            data = data.OrderByDescending(mcp => mcp.Capacity);
                        break;
                    case SortBy.CurrentLoad:
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