using Commons.Models;
using Repositories.Generics;
using Repositories.Managers;

namespace Repositories.Implementations.Mcps.McpFillLevelLogs;

public class McpFillLevelLogRepository : GenericRepository<McpFillLevelLog>, IMcpFillLevelLogRepository
{
    public McpFillLevelLogRepository(UwcDbContext context) : base(context)
    {
    }

    public List<McpFillLevelLog> GetLogsByDate(DateTime date)
    {
        return Context.McpFillLevelLogTable.Where(log => log.Timestamp.Date == date.Date).ToList();
    }
}