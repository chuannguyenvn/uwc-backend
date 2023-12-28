using Commons.Models;
using Repositories.Generics;

namespace Repositories.Implementations.Mcps.McpEmptyRecords;

public interface IMcpEmptyRecordRepository : IGenericRepository<McpEmptyRecord>
{
    public List<McpEmptyRecord> GetRecordsByDate(DateTime date);
    public List<McpEmptyRecord> GetRecordsInTimeSpan(DateTime startDate, DateTime endDate);
}