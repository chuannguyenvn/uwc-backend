using Commons.Categories;
using Commons.Communications.Reports;
using Commons.RequestStatuses;
using Commons.Types;
using Hubs;
using Repositories.Managers;

namespace Services.Reports;

public class ReportService : IReportService
{
    private readonly UnitOfWork _unitOfWork;

    public ReportService(UnitOfWork unitOfWork)
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
        var tasks = _unitOfWork.TaskDatas.GetTasksByDate(DateTime.Now);
        response.TotalTasksCreated = tasks.Count;
        response.TotalTasksCompleted = tasks.Count(t => t.IsCompleted);
        response.AverageTaskCompletionTimeInMinutes = (float)(tasks.Count == 0
            ? 0
            : tasks.Average(taskData =>
            {
                var completedTimestamp = taskData.CompletedTimestamp ?? DateTime.Now;
                var assignedTimestamp = taskData.AssignedTimestamp;
                return (completedTimestamp - assignedTimestamp).Minutes;
            }));
    }

    private void CalculateWorkerOnlineMetrics(GetDashboardReportResponse response)
    {
        var allCleaners = _unitOfWork.UserProfiles.GetByUserRole(UserRole.Cleaner);
        var allDrivers = _unitOfWork.UserProfiles.GetByUserRole(UserRole.Driver);
        var allWorkers = allCleaners.Concat(allDrivers).ToList();
        var allConnectedAccountIds = BaseHub.ConnectionIds.Keys;
        var onlineWorkers = allWorkers.Where(w => allConnectedAccountIds.Contains(w.AccountId)).ToList();
        response.TotalWorkers = allWorkers.Count;
        response.OnlineWorkers = onlineWorkers.Count;
    }

    private void CalculateMcpFillLevelMetrics(GetDashboardReportResponse response)
    {
        var fillLevelLogs = _unitOfWork.McpFillLevelLogs.GetLogsByDate(DateTime.Now);
        response.TotalMcpFillLevelTimestamps = fillLevelLogs.Select(m => m.Timestamp).ToList();
        response.TotalMcpFillLevelValues = fillLevelLogs.Select(m => m.McpFillLevel).ToList();

        var mcpEmptied = _unitOfWork.McpEmptyRecords.GetRecordsByDate(DateTime.Now);
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