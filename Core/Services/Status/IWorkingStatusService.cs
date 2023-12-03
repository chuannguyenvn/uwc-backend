using Commons.Communications.Status;

namespace Services.Status;

public interface IWorkingStatusService
{
    public ParamRequestResult<GetWorkingStatusResponse> GetWorkingStatus(GetWorkingStatusRequest request);
}