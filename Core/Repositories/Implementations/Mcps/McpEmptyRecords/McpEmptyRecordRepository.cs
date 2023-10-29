using Commons.Models;
using Repositories.Generics;
using Repositories.Managers;

namespace Repositories.Implementations.Mcps.McpEmptyRecords;

public class McpEmptyRecordRepository : GenericRepository<McpEmptyRecord>, IMcpEmptyRecordRepository
{
    public McpEmptyRecordRepository(UwcDbContext context) : base(context)
    {
    }

    public List<McpEmptyRecord> GetRecordsByDate(DateTime date)
    {
        return Context.McpEmptyRecordTable.Where(record => record.Timestamp.Date == date.Date).ToList();
    }
}