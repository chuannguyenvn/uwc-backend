using Commons.Categories;
using Commons.Models;
using Commons.Types;

namespace Repositories.Implementations;

public class McpDataRepository : GenericRepository<McpData>
{
    public McpDataRepository(UwcDbContext context) : base(context)
    {
    }

    public IEnumerable<McpData> GetData(McpQueryParameters parameters)
    {
        var enumerable = _context.Set<McpData>().AsEnumerable();
        return parameters.Execute(enumerable);
    }
}