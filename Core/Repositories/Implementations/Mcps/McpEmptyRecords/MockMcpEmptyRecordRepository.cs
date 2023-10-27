using Commons.Models;
using Repositories.Generics;
using Repositories.Managers;

namespace Repositories.Implementations.Mcps.McpEmptyRecords;

public class MockMcpEmptyRecordRepository : MockGenericRepository<McpEmptyRecord>, IMcpEmptyRecordRepository
{
    public MockMcpEmptyRecordRepository(MockUwcDbContext context) : base(context)
    {
    }
}