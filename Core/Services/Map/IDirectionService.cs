using Commons.Communications.Map;
using Commons.Types;

namespace Services.Map;

public interface IDirectionService
{
    public ParamRequestResult<GetDirectionResponse> GetDirection(GetDirectionRequest request);
    public RawMapboxDirectionResponse GetRawDirection(Coordinate from, Coordinate to);
}