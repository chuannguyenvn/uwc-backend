using Commons.Types;
using Repositories.Generics;
using Repositories.Managers;

namespace Repositories.Implementations.Mcps.McpData;

public class McpDataRepository : GenericRepository<Commons.Models.McpData>, IMcpDataRepository
{
    public McpDataRepository(UwcDbContext context) : base(context)
    {
    }

    public IEnumerable<Commons.Models.McpData> GetData(McpDataQueryParameters parameters)
    {
        return parameters.Execute(Context.Set<Commons.Models.McpData>());
    }
}