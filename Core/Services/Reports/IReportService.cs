using Commons.Communications.Reports;

namespace Services.Reports;

public interface IReportService
{
    public ParamRequestResult<GetDashboardReportResponse> GetTodayDashboardReport();
}