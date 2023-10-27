using Commons.Communications.Mcps;

namespace Services.Mcps;

public interface IMcpFillLevelService : IHostedService, IDisposable
{
    public Dictionary<int, float> FillLevelsById { get; }
    public ParamRequestResult<GetFillLevelResponse> GetFillLevel(GetFillLevelRequest request);
    public ParamRequestResult<GetFillLevelResponse> GetAllFillLevel();
    public RequestResult EmptyMcp(EmptyMcpRequest request);
}