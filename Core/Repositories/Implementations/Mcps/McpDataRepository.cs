using Repositories.Generics;
using Repositories.Managers;
using Commons.Models;
using Commons.Types;

namespace Repositories.Implementations.Mcps;

public class McpDataRepository : GenericRepository<McpData>, IMcpDataRepository
{
    public McpDataRepository(UwcDbContext context) : base(context)
    {
    }

    public IEnumerable<McpData> GetData(McpDataQueryParameters parameters)
    {
        return parameters.Execute(Context.Set<McpData>());
    }
}