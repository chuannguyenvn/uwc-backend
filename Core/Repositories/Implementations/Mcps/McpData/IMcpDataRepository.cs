using Commons.Models;
using Commons.Types;
using Repositories.Generics;

namespace Repositories.Implementations.Mcps.McpData;

public interface IMcpDataRepository : IGenericRepository<Commons.Models.McpData>
{
    public Commons.Models.McpData GetSingle(int mcpId, int countLimit, DateTime dateTimeLimit);
    public IEnumerable<Commons.Models.McpData> GetData(McpDataQueryParameters parameters);
    public IEnumerable<McpEmptyRecord> GetEmptyRecords(int mcpId, int countLimit, DateTime dateTimeLimit);
    public IEnumerable<McpFillLevelLog> GetFillLevelLogs(int mcpId, int countLimit, DateTime dateTimeLimit);
}