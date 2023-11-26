using Commons.Categories;
using Commons.Communications.Reports;
using Commons.RequestStatuses;
using Commons.Types;
using Hubs;
using Repositories.Managers;
using TaskStatus = Commons.Types.TaskStatus;

namespace Services.Reports;

public class ReportService : IReportService
{
    private readonly IUnitOfWork _unitOfWork;

    public ReportService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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

    private void CalculateTaskMetrics(GetDashboardReportResponse response)
    {
        var tasks = _unitOfWork.TaskDataDataRepository.GetTasksByDate(DateTime.Now);
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
        var fillLevelLogs = _unitOfWork.McpFillLevelLogRepository.GetLogsByDate(DateTime.Now);
        response.TotalMcpFillLevelTimestamps = fillLevelLogs.Select(m => m.Timestamp).ToList();
        response.TotalMcpFillLevelValues = fillLevelLogs.Select(m => m.McpFillLevel).ToList();

        var mcpEmptied = _unitOfWork.McpEmptyRecordRecordRepository.GetRecordsByDate(DateTime.Now);
        response.McpEmptiedTimestamps = mcpEmptied.Select(m => m.Timestamp).ToList();
    }

    private void CalculateWeatherMetrics(GetDashboardReportResponse response)
    {
        var random = new Random();
        response.CurrentTemperature = random.Next(0, 40);
        response.ChanceOfPrecipitation = random.Next(0, 4) switch
        {
            0 => ChanceOfPrecipitation.None,
            1 => ChanceOfPrecipitation.Slight,
            2 => ChanceOfPrecipitation.Moderate,
            _ => ChanceOfPrecipitation.High,
        };
    }
}