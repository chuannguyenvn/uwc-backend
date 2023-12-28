using Commons.Models;
using Microsoft.EntityFrameworkCore;
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
        return Context.McpEmptyRecordTable.Where(record => record.Timestamp.Date == date.Date).Include(record => record.McpData)
            .Include(record => record.EmptyingWorker).ToList();
    }
    
    public List<McpEmptyRecord> GetRecordsInTimeSpan(DateTime startDate, DateTime endDate)
    {
        return Context.McpEmptyRecordTable.Where(record => record.Timestamp >= startDate && record.Timestamp <= endDate).Include(record => record.McpData)
            .Include(record => record.EmptyingWorker).ToList();
    }
}