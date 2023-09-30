using Commons.Communications.Mcps;
using Commons.Types;

namespace Services.Mcps;

public interface IMcpFillLevelService
{
    public ParamRequestResult<GetFillLevelResponse> GetAllFillLevel();
}