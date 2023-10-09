using Commons.Communications.Mcps;
using Commons.Types;

namespace Services.Mcps;

public interface IMcpFillLevelService : IHostedService, IDisposable
{
    public ParamRequestResult<GetFillLevelResponse> GetAllFillLevel();
}