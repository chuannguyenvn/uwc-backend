using Commons.Communications.Map;

namespace Services.Map;

public interface IDirectionService
{
    public ParamRequestResult<GetDirectionResponse> GetDirection(GetDirectionRequest request);
}