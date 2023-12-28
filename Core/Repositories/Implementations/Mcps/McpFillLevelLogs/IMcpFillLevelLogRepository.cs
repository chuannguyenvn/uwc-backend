using Commons.Models;
using Repositories.Generics;

namespace Repositories.Implementations.Mcps.McpFillLevelLogs;

public interface IMcpFillLevelLogRepository : IGenericRepository<McpFillLevelLog>
{
    public List<McpFillLevelLog> GetLogsByDate(DateTime date);
    public List<McpFillLevelLog> GetLogsInTimeSpan(DateTime startDate, DateTime endDate);
}