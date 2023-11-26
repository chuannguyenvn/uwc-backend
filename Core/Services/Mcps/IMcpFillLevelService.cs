using Commons.Communications.Mcps;

namespace Services.Mcps;

public interface IMcpFillLevelService
{
    /// <summary>
    /// Fill level is a float between 0 and 1.
    /// </summary>
    public ParamRequestResult<GetFillLevelResponse> GetFillLevel(GetFillLevelRequest request);

    /// <summary>
    /// Fill level is a float between 0 and 1.
    /// </summary>
    public ParamRequestResult<GetFillLevelResponse> GetAllFillLevel();

    /// <summary>
    /// Fill level is a float between 0 and 1.
    /// </summary>
    public RequestResult SetFillLevel(SetFillLevelRequest request);
    public RequestResult EmptyMcp(EmptyMcpRequest request);
}