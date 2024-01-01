using Commons.Categories;
using Commons.Communications.Reports;
using Commons.RequestStatuses;
using Commons.Types;
using Hubs;
using Newtonsoft.Json;
using Repositories.Managers;
using Services.Mcps;
using TaskStatus = Commons.Types.TaskStatus;

namespace Services.Reports;

public class ReportService : IReportService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly McpFillLevelService _mcpFillLevelService;

    public ReportService(IUnitOfWork unitOfWork, McpFillLevelService mcpFillLevelService)
    {
        _unitOfWork = unitOfWork;
        _mcpFillLevelService = mcpFillLevelService;
    }

    public ParamRequestResult<GetDashboardReportResponse> GetTodayDashboardReport()
    {
        var response = new GetDashboardReportResponse();

        CalculateTaskMetrics(response);
        CalculateWorkerOnlineMetrics(response);
        CalculateMcpFillLevelMetrics(response);
        CalculateWeatherMetrics(response);

        return new ParamRequestResult<GetDashboardReportResponse>(new Success(), response);
    }

    public ParamRequestResult<GetReportFileResponse> GetReportFile(GetReportFileRequest request)
    {
        var response = new GetReportFileResponse();

        var fillLevelLogs = _unitOfWork.McpFillLevelLogRepository.GetLogsInTimeSpan(DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);
        fillLevelLogs = fillLevelLogs.OrderBy(m => m.Timestamp).ToList();
        response.McpFillLevelTimestamps = fillLevelLogs.Select(m => m.Timestamp).ToList();
        response.FillLevelMcpIds = fillLevelLogs.Select(m => m.McpDataId).ToList();
        response.FillLevelMcpAddresses = fillLevelLogs.Select(m => m.McpData.Address).ToList();
        response.McpFillLevelValues = fillLevelLogs.Select(m => m.McpFillLevel).ToList();

        var mcpEmptied = _unitOfWork.McpEmptyRecordRecordRepository.GetRecordsInTimeSpan(DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);
        mcpEmptied = mcpEmptied.OrderBy(m => m.Timestamp).ToList();
        response.McpEmptiedTimestamps = mcpEmptied.Select(m => m.Timestamp).ToList();
        response.EmptyingMcpIds = mcpEmptied.Select(m => m.McpDataId).ToList();
        response.EmptyingMcpAddresses = mcpEmptied.Select(m => m.McpData.Address).ToList();
        response.WorkerIds = mcpEmptied.Select(m => m.EmptyingWorkerId).ToList();
        response.WorkerNames = mcpEmptied.Select(m => m.EmptyingWorker.FirstName + " " + m.EmptyingWorker.FirstName).ToList();

        return new ParamRequestResult<GetReportFileResponse>(new Success(), response);
    }

    private void CalculateTaskMetrics(GetDashboardReportResponse response)
    {
        var tasks = _unitOfWork.TaskDataDataRepository.GetTasksByDate(DateTime.UtcNow);
        response.TotalTasksCreated = tasks.Count;
        response.TotalTasksCompleted = tasks.Count(t => t.TaskStatus == TaskStatus.Completed);
        response.AverageTaskCompletionTimeInMinutes = (float)(tasks.Count == 0
            ? 0
            : tasks.Where(data => data.TaskStatus == TaskStatus.Completed).Average(taskData =>
            {
                var completedTimestamp = taskData.LastStatusChangeTimestamp;
                var assignedTimestamp = taskData.CompleteByTimestamp;
                return (completedTimestamp - assignedTimestamp).Minutes;
            }));
    }

    private void CalculateWorkerOnlineMetrics(GetDashboardReportResponse response)
    {
        var allCleaners = _unitOfWork.UserProfileRepository.GetByUserRole(UserRole.Cleaner);
        var allDrivers = _unitOfWork.UserProfileRepository.GetByUserRole(UserRole.Driver);
        var allWorkers = allCleaners.Concat(allDrivers).ToList();
        var allConnectedAccountIds = BaseHub.ConnectionIds.Keys;
        var onlineWorkers = allWorkers.Where(w => allConnectedAccountIds.Contains(w.AccountId)).ToList();
        response.TotalWorkers = allWorkers.Count;
        response.OnlineWorkers = onlineWorkers.Count;
    }

    private void CalculateMcpFillLevelMetrics(GetDashboardReportResponse response)
    {
        if (_mcpFillLevelService.GetAllFillLevel().Data != null)
            response.AverageMcpCapacity = _mcpFillLevelService.GetAllFillLevel().Data.FillLevelsById.Values.Average();

        var fillLevelLogs = _unitOfWork.McpFillLevelLogRepository.GetLogsInTimeSpan(DateTime.UtcNow.AddHours(-24), DateTime.UtcNow);
        fillLevelLogs = fillLevelLogs.OrderBy(m => m.Timestamp).ToList();
        response.TotalMcpFillLevelTimestamps = fillLevelLogs.Select(m => m.Timestamp).ToList();
        response.TotalMcpFillLevelValues = fillLevelLogs.Select(m => m.McpFillLevel).ToList();

        var mcpEmptied = _unitOfWork.McpEmptyRecordRecordRepository.GetRecordsInTimeSpan(DateTime.UtcNow.AddHours(-24), DateTime.UtcNow);
        mcpEmptied = mcpEmptied.OrderBy(m => m.Timestamp).ToList();
        response.McpEmptiedTimestamps = mcpEmptied.Select(m => m.Timestamp).ToList();
    }

    private void CalculateWeatherMetrics(GetDashboardReportResponse response)
    {
        var weather = RequestWeather();

        response.CurrentTemperature = (float)weather.Current.Temp;

        var maxPrecipitation = weather.Hourly.Take(6).Max(h => h.Pop);
        response.ChanceOfPrecipitation = maxPrecipitation switch
        {
            < 0.25 => ChanceOfPrecipitation.None,
            < 0.5 => ChanceOfPrecipitation.Slight,
            < 0.75 => ChanceOfPrecipitation.Moderate,
            _ => ChanceOfPrecipitation.High,
        };
    }

    private OpenWeatherResponse RequestWeather()
    {
        var client = new HttpClient();
        var httpResponse = client.GetStringAsync(Constants.OPEN_WEATHER_API).Result;
        var openWeatherResponse = JsonConvert.DeserializeObject<OpenWeatherResponse>(httpResponse);
        return openWeatherResponse;
    }
}