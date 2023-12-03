using Commons.Communications.Map;
using Commons.Communications.Status;
using Commons.RequestStatuses;
using Hubs;
using Repositories.Managers;
using Services.Map;

namespace Services.Status;

public class WorkingStatusService : IWorkingStatusService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDirectionService _directionService;
    private readonly ILocationService _locationService;

    public WorkingStatusService(IUnitOfWork unitOfWork, IDirectionService directionService, ILocationService locationService)
    {
        _unitOfWork = unitOfWork;
        _directionService = directionService;
        _locationService = locationService;
    }

    public ParamRequestResult<GetWorkingStatusResponse> GetWorkingStatus(GetWorkingStatusRequest request)
    {
        if (!_unitOfWork.UserProfileRepository.DoesIdExist(request.WorkerId))
            return new ParamRequestResult<GetWorkingStatusResponse>(new DataEntryNotFound());

        var getWorkingStatusResponse = new GetWorkingStatusResponse();

        var profile = _unitOfWork.UserProfileRepository.GetById(request.WorkerId);
        profile.Account = null;

        getWorkingStatusResponse.IsOnline = BaseHub.ConnectionIds.Keys.Contains(profile.Id);

        var focusedTask = _unitOfWork.TaskDataDataRepository.GetFocusedTaskByWorkerId(request.WorkerId);
        if (focusedTask != null)
        {
            focusedTask.AssignerProfile.Account = null;
            if (focusedTask.AssigneeProfile != null) focusedTask.AssigneeProfile.Account = null;
        }

        getWorkingStatusResponse.FocusedTask = focusedTask;

        if (focusedTask != null)
        {
            var fromLocation = _locationService.GetLocation(new GetLocationRequest() { AccountId = request.WorkerId }).Data.Coordinate;
            var toLocation = focusedTask.McpData.Coordinate;
            getWorkingStatusResponse.DirectionToFocusedTask = _directionService.GetRawDirection(fromLocation, toLocation);
        }

        return new ParamRequestResult<GetWorkingStatusResponse>(new Success(), getWorkingStatusResponse);
    }
}