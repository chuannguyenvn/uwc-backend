using Commons.Communications.Mcps;
using Commons.Types;

namespace Services.Mcps;

public interface IMcpDataService
{
    public RequestResult AddNewMcp(AddNewMcpRequest request);
    public RequestResult GetAllStableData(McpQueryParameters parameters);
    public RequestResult GetAllVolatileData(McpQueryParameters parameters);
}