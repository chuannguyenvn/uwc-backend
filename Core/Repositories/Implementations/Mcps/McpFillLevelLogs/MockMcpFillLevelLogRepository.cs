using Commons.Models;
using Repositories.Generics;
using Repositories.Managers;

namespace Repositories.Implementations.Mcps.McpFillLevelLogs;

public class MockMcpFillLevelLogRepository : MockGenericRepository<McpFillLevelLog>, IMcpFillLevelLogRepository
{
    public MockMcpFillLevelLogRepository(MockUwcDbContext context) : base(context)
    {
    }

    public List<McpFillLevelLog> GetLogsByDate(DateTime date)
    {
        return Context.McpFillLevelLogs.Where(log => log.Timestamp.Date == date.Date).ToList();
    }
}