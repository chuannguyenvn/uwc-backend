using Commons.Models;
using Commons.Types;
using Repositories.Generics;
using Repositories.Managers;

namespace Repositories.Implementations.Mcps.McpData;

public class MockMcpDataRepository : MockGenericRepository<Commons.Models.McpData>, IMcpDataRepository
{
    public MockMcpDataRepository(MockUwcDbContext context) : base(context)
    {
    }

    public IEnumerable<Commons.Models.McpData> GetData(McpDataQueryParameters parameters)
    {
        var enumerable = Context.Set<Commons.Models.McpData>().AsEnumerable();
        return parameters.Execute(enumerable);
    }

    public Commons.Models.McpData GetSingle(int mcpId, int countLimit, DateTime dateTimeLimit)
    {
        var mcp = Context.McpDataTable.First(data => data.Id == mcpId);
        mcp.McpEmptyRecords = GetEmptyRecords(mcpId, countLimit, dateTimeLimit).ToList();
        mcp.McpFillLevelLogs = GetFillLevelLogs(mcpId, countLimit, dateTimeLimit).ToList();
        return mcp;
    }

    public IEnumerable<McpEmptyRecord> GetEmptyRecords(int mcpId, int countLimit, DateTime dateTimeLimit)
    {
        var rawRecords = GetById(mcpId).McpEmptyRecords.TakeLast(countLimit).Where(record => record.Timestamp >= dateTimeLimit);
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
        var rawRecords = GetById(mcpId).McpFillLevelLogs.TakeLast(countLimit).Where(record => record.Timestamp >= dateTimeLimit);
        var trimmedRecords = new List<McpFillLevelLog>();

        foreach (var record in rawRecords)
        {
            record.McpData = null!;
            trimmedRecords.Add(record);
        }

        return trimmedRecords;
    }
}