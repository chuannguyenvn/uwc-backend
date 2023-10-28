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
        return Context.McpEmptyRecords.Where(record => record.Timestamp.Date == date.Date).ToList();
    }
}