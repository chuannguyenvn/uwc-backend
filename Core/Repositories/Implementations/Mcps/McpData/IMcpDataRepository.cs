using Commons.Types;
using Repositories.Generics;

namespace Repositories.Implementations.Mcps.McpData;

public interface IMcpDataRepository : IGenericRepository<Commons.Models.McpData>
{
    public IEnumerable<Commons.Models.McpData> GetData(McpDataQueryParameters parameters);
}