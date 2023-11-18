using Commons.Communications.Mcps;
using Commons.Types;

namespace Services.Mcps;

public interface IMcpDataService
{
    public RequestResult AddNewMcp(AddNewMcpRequest request);
    public RequestResult UpdateMcp(UpdateMcpRequest request);
    public RequestResult RemoveMcp(RemoveMcpRequest request);
    public ParamRequestResult<GetSingleMcpDataResponse> GetSingleMcpData(GetSingleMcpDataRequest request);
    public ParamRequestResult<GetMcpDataResponse> GetMcpData(McpDataQueryParameters parameters);
    public ParamRequestResult<GetEmptyRecordsResponse> GetEmptyRecords(GetEmptyRecordsRequest request);
    public ParamRequestResult<GetFillLevelLogsResponse> GetFillLevelLogs(GetFillLevelLogsRequest request);
}