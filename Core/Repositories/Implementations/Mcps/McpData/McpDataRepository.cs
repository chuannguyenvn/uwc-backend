using Commons.Models;
using Commons.Types;
using Microsoft.EntityFrameworkCore;
using Repositories.Generics;
using Repositories.Managers;

namespace Repositories.Implementations.Mcps.McpData;

public class McpDataRepository : GenericRepository<Commons.Models.McpData>, IMcpDataRepository
{
    public McpDataRepository(UwcDbContext context) : base(context)
    {
    }

    public Commons.Models.McpData GetSingle(int mcpId, int countLimit, DateTime dateTimeLimit)
    {
        var mcp = Context.McpDataTable.First(data => data.Id == mcpId);
        mcp.McpEmptyRecords = GetEmptyRecords(mcpId, countLimit, dateTimeLimit).ToList();
        mcp.McpFillLevelLogs = GetFillLevelLogs(mcpId, countLimit, dateTimeLimit).ToList();
        return mcp;
    }

    public IEnumerable<Commons.Models.McpData> GetData(McpDataQueryParameters parameters)
    {
        return parameters.Execute(Context.McpDataTable);
    }

    public IEnumerable<McpEmptyRecord> GetEmptyRecords(int mcpId, int countLimit, DateTime dateTimeLimit)
    {
        var rawRecords = Context.McpDataTable.Include(data => data.McpEmptyRecords).ThenInclude(record => record.EmptyingWorker)
            .First(data => data.Id == mcpId).McpEmptyRecords
            .TakeLast(countLimit).Where(record => record.Timestamp >= dateTimeLimit);
        var trimmedRecords = new List<McpEmptyRecord>();

        foreach (var record in rawRecords)
        {
            record.McpData = null!;
            trimmedRecords.Add(record);
        }

        return trimmedRecords;
    }

    public IEnumerable<McpFillLevelLog> GetFillLevelLogs(int mcpId, int countLimit, DateTime dateTimeLimit)
    {
        var rawRecords = Context.McpDataTable.Include(data => data.McpFillLevelLogs).First(data => data.Id == mcpId).McpFillLevelLogs
            .TakeLast(countLimit).Where(record => record.Timestamp >= dateTimeLimit);
        var trimmedRecords = new List<McpFillLevelLog>();

        foreach (var record in rawRecords)
        {
            record.McpData = null!;
            trimmedRecords.Add(record);
        }

        return trimmedRecords;
    }
}