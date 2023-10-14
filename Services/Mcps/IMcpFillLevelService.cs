using Commons.Communications.Mcps;

namespace Services.Mcps;

public interface IMcpFillLevelService : IHostedService, IDisposable
{
    public ParamRequestResult<GetFillLevelResponse> GetAllFillLevel();
}