using Commons.Communications.Map;
using Commons.Models;
using Commons.Types;
using Newtonsoft.Json;
using Repositories.Managers;
using Services.Map;
using Services.Mcps;
using Services.Status;

namespace Services.Tasks;

public class TaskOptimizationServiceHelper
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITaskService _taskService;
    private readonly ILocationService _locationService;
    private readonly IMcpFillLevelService _mcpFillLevelService;
    private readonly IOnlineStatusService _onlineStatusService;

    public TaskOptimizationServiceHelper(IUnitOfWork unitOfWork, ITaskService taskService, ILocationService locationService,
        IMcpFillLevelService mcpFillLevelService, IOnlineStatusService onlineStatusService)
    {
        _unitOfWork = unitOfWork;
        _taskService = taskService;
        _locationService = locationService;
        _mcpFillLevelService = mcpFillLevelService;
        _onlineStatusService = onlineStatusService;
    }

    public List<TaskData> GetWorkerTasksIn24Hours(int workerId)
    {
        return _unitOfWork.TaskDataDataRepository.GetWorkerRemainingTasksIn24Hours(workerId);
    }

    public List<TaskData> GetUnassignedTaskIn24Hours()
    {
        return _unitOfWork.TaskDataDataRepository.GetUnassignedTasksIn24Hours();
    }

    public Dictionary<int, float> GetAllMcpFillLevels()
    {
        return _mcpFillLevelService.GetAllFillLevel().Data.FillLevelsById;
    }

    public Coordinate GetMcpCoordinateById(int mcpId)
    {
        return _unitOfWork.McpDataRepository.GetById(mcpId).Coordinate;
    }

    public Coordinate GetWorkerLocation(int workerId)
    {
        return _locationService.GetLocation(new GetLocationRequest
        {
            AccountId = workerId
        }).Data!.Coordinate;
    }

    public Dictionary<int, UserProfile> GetAllWorkerProfiles()
    {
        return _unitOfWork.UserProfileRepository.GetAllWorkers().ToDictionary(workerProfile => workerProfile.Id);
    }

    public bool IsWorkerOnline(int workerId)
    {
        return _onlineStatusService.IsAccountOnline(workerId);
    }

    private string ConstructMapboxMatrixRequest(List<Coordinate> coordinates)
    {
        return string.Format(Constants.MAPBOX_MATRIX_API, String.Join(';', coordinates.Select(location => location.ToStringApi())));
    }

    public MapboxMatrixResponse RequestMapboxMatrix(List<Coordinate> coordinates)
    {
        var client = new HttpClient();
        var httpResponse = client.GetStringAsync(ConstructMapboxMatrixRequest(coordinates)).Result;
        var mapboxMatrixResponse = JsonConvert.DeserializeObject<MapboxMatrixResponse>(httpResponse);
        return mapboxMatrixResponse;
    }
}