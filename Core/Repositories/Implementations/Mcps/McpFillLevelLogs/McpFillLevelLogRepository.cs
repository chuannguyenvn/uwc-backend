using Commons.Models;
using Microsoft.EntityFrameworkCore;
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
        return Context.McpFillLevelLogTable.Where(log => log.Timestamp.Date == date.Date).Include(log => log.McpData).ToList();
    }

    public List<McpFillLevelLog> GetLogsInTimeSpan(DateTime startDate, DateTime endDate)
    {
        return Context.McpFillLevelLogTable.Where(log => log.Timestamp >= startDate && log.Timestamp <= endDate).Include(log => log.McpData).ToList();
    }
}