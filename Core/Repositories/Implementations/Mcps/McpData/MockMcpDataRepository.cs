using Commons.Types;
using Repositories.Generics;
using Repositories.Managers;

namespace Repositories.Implementations.Mcps.McpData;

public class MockMcpDataRepository : MockGenericRepository<Commons.Models.McpData>, IMcpDataRepository
{
    public MockMcpDataRepository(MockUwcDbContext context) : base(context)
    {
    }

    public IEnumerable<Commons.Models.McpData> GetData(McpDataQueryParameters parameters)
    {
        var enumerable = Context.Set<Commons.Models.McpData>().AsEnumerable();
        return parameters.Execute(enumerable);
    }
}