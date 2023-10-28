using Microsoft.AspNetCore.Mvc;
using Commons.Endpoints;
using Microsoft.AspNetCore.Authorization;
using Services.Reports;

namespace Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ReportController : Controller
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet(Endpoints.Report.GET)]
    public IActionResult Get()
    {
        var result = _reportService.GetTodayDashboardReport();
        return ProcessRequestResult(result);
    }
}