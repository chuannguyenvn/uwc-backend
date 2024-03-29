﻿using Commons.Models;
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
        return Context.McpFillLevelLogTable.Where(log => log.Timestamp.Date == date.Date).ToList();
    }

    public List<McpFillLevelLog> GetLogsInTimeSpan(DateTime startDate, DateTime endDate)
    {
        return Context.McpFillLevelLogTable.Where(log => log.Timestamp >= startDate && log.Timestamp <= endDate).ToList();
    }
}