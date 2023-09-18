﻿using System;
using System.Collections.Generic;
using System.Linq;
using Commons.Categories;
using Commons.Models;

namespace Commons.Types
{
    public class McpQueryParameters
    {
        public FilterBy Filter { get; } = new FilterBy();
        public Dictionary<SortBy, SortStrategy> Sort { get; } = new Dictionary<SortBy, SortStrategy>();
        public int PageIndex { get; set; } = 0;

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
}