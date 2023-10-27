using Commons.Models;
using Repositories.Generics;
using Repositories.Managers;

namespace Repositories.Implementations.Mcps.McpFillLevelLogs;

public class McpFillLevelLogRepository : GenericRepository<McpFillLevelLog>, IMcpFillLevelLogRepository
{
    public McpFillLevelLogRepository(UwcDbContext context) : base(context)
    {
    }
}