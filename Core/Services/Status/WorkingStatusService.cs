using Commons.Communications.Status;
using Commons.RequestStatuses;
using Hubs;
using Repositories.Managers;

namespace Services.Status;

public class WorkingStatusService : IWorkingStatusService
{
    private readonly IUnitOfWork _unitOfWork;

    public WorkingStatusService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public ParamRequestResult<GetWorkingStatusResponse> GetWorkingStatus(GetWorkingStatusRequest request)
    {
        if (!_unitOfWork.UserProfileRepository.DoesIdExist(request.WorkerId))
            return new ParamRequestResult<GetWorkingStatusResponse>(new DataEntryNotFound());

        var profile = _unitOfWork.UserProfileRepository.GetById(request.WorkerId);
        profile.Account = null;

        var focusedTask = _unitOfWork.TaskDataDataRepository.GetFocusedTaskByWorkerId(request.WorkerId);
        if (focusedTask != null)
        {
            focusedTask.AssignerProfile.Account = null;
            if (focusedTask.AssigneeProfile != null) focusedTask.AssigneeProfile.Account = null;
        }

        var getWorkingStatusResponse = new GetWorkingStatusResponse()
        {
            IsOnline = BaseHub.ConnectionIds.Keys.Contains(profile.Id),
            UserProfile = profile,
            FocusedTask = focusedTask,
        };

        return new ParamRequestResult<GetWorkingStatusResponse>(new Success(), getWorkingStatusResponse);
    }
}