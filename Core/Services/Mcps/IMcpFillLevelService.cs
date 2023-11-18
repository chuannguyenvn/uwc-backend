using Commons.Communications.Mcps;

namespace Services.Mcps;

public interface IMcpFillLevelService : IHostedService, IDisposable
{
    public ParamRequestResult<GetFillLevelResponse> GetFillLevel(GetFillLevelRequest request);
    public ParamRequestResult<GetFillLevelResponse> GetAllFillLevel();
    public RequestResult SetFillLevel(SetFillLevelRequest request);
    public RequestResult EmptyMcp(EmptyMcpRequest request);
}