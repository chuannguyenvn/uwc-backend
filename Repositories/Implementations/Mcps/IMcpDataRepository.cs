using Repositories.Generics;
using Commons.Models;
using Commons.Types;

namespace Repositories.Implementations.Mcps;

public interface IMcpDataRepository : IGenericRepository<McpData>
{
    public IEnumerable<McpData> GetData(McpQueryParameters parameters);
}