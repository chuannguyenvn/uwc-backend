using Commons.Models;
using Repositories.Generics;
using Repositories.Managers;

namespace Repositories.Implementations.Mcps.McpEmptyRecords;

public class MockMcpEmptyRecordRepository : MockGenericRepository<McpEmptyRecord>, IMcpEmptyRecordRepository
{
    public MockMcpEmptyRecordRepository(MockUwcDbContext context) : base(context)
    {
    }

    public List<McpEmptyRecord> GetRecordsByDate(DateTime date)
    {
        return Context.McpEmptyRecordTable.Where(record => record.Timestamp.Date == date.Date).ToList();
    }

    public List<McpEmptyRecord> GetRecordsInTimeSpan(DateTime startDate, DateTime endDate)
    {
        return Context.McpEmptyRecordTable.Where(record => record.Timestamp >= startDate && record.Timestamp <= endDate).ToList();
    }
}