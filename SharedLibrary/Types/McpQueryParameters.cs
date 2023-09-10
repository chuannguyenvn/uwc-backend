using Commons.Categories;
using Commons.Models;

namespace Commons.Types;

public class McpQueryParameters
{
    public FilterBy Filter { get; init; }
    public Dictionary<SortBy, SortStrategy> Sort { get; init; } = new();
    public int PageIndex { get; init; } = 0;

    public enum SortBy
    {
        Address,
        Capacity,
        CurrentLoad,
        CurrentLoadPercentage,
    }

    public class FilterBy
    {
        public Zone? Zone { get; init; } = null;
        public Range<float>? CapacityRange { get; init; } = null;
        public Range<float>? CurrentLoadRange { get; init; } = null;
        public Range<float>? CurrentLoadPercentageRange { get; init; } = null;
    }

    public IEnumerable<McpData> Execute(IEnumerable<McpData> startingData)
    {
        var result = startingData;

        if (Filter.Zone != null)
        {
            result = result.Where(mcp => mcp.Zone == Filter.Zone);
        }

        if (Filter.CapacityRange != null)
        {
            result = result.Where(mcp => Filter.CapacityRange.Value.Contains(mcp.Capacity));
        }

        // if (parameters.Filter.CurrentLoadRange != null)
        // {
        //     result = result.Where(mcp => parameters.Filter.CurrentLoadRange.Value.Contains(mcp.CurrentLoad));
        // }

        // if (parameters.Filter.CurrentLoadPercentageRange != null)
        // {
        //     result = result.Where(mcp => parameters.Filter.CurrentLoadPercentageRange.Value.Contains(mcp.CurrentLoadPercentage));
        // }

        foreach (var (sortBy, strategy) in Sort)
        {
            switch (sortBy)
            {
                case SortBy.Address:
                    if (strategy == SortStrategy.Ascending)
                        result = result.OrderBy(mcp => mcp.Address);
                    else
                        result = result.OrderByDescending(mcp => mcp.Address);
                    break;
                case SortBy.Capacity:
                    if (strategy == SortStrategy.Ascending)
                        result = result.OrderBy(mcp => mcp.Capacity);
                    else
                        result = result.OrderByDescending(mcp => mcp.Capacity);
                    break;
                case SortBy.CurrentLoad:
                    break;
                case SortBy.CurrentLoadPercentage:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return result;
    }
}