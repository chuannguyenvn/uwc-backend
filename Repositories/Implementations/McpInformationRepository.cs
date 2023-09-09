using Commons.Models;

namespace Repositories.Implementations;

public class McpInformationRepository : GenericRepository<McpInformation>
{
    public McpInformationRepository(UwcDbContext context) : base(context)
    {
    }
}