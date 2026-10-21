using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Reporting.NETCore;
using Payroll_Management_System.Helpers;
using PayrollManagementSystem.Business.Interfaces;

namespace Payroll_Management_System.Controllers
{
    public class AttendanceRdlcReportController : Controller
    {
        // Embedded resource name = RootNamespace + folder + file name
        private const string ReportDefinition = "Payroll_Management_System.Reports.Attendance.rdlc";

        private readonly IReportService _reports;

        public AttendanceRdlcReportController(IReportService reports)
        {
            _reports = reports;
        }

        // Filter page; the report frame appears once the filter is submitted (month and department in the URL)
        [HttpGet]
        public async Task<IActionResult> Index(string? month, int? departmentId, CancellationToken cancellationToken)
        {

            var monthStart = ReportMonth.Parse(month);

            var departments = await _reports.GetDepartmentsLookupAsync(cancellationToken);
            ViewBag.Departments = new SelectList(departments, "Id", "Name", departmentId);
            ViewData["Month"] = monthStart.ToString("yyyy-MM");
            ViewData["MaxMonth"] = ReportMonth.Current().ToString("yyyy-MM");
            ViewData["DepartmentId"] = departmentId;
            ViewData["ShowReport"] = month is not null && departmentId is not null;

            return View();
        }

        // Returns the PDF inline (no file name) so the browser displays it instead of downloading
        [HttpGet]
        public async Task<IActionResult> Render(string? month, int departmentId, CancellationToken cancellationToken)
        {
            // The department is required
            var departmentName = await _reports.GetDepartmentNameAsync(departmentId, cancellationToken);
            if (departmentName is null)
            {
                return NotFound();
            }

            var monthStart = ReportMonth.Parse(month);

            var rows = await _reports.GetAttendanceAsync(monthStart, departmentId, cancellationToken);

            using var definition = typeof(AttendanceRdlcReportController).Assembly.GetManifestResourceStream(ReportDefinition)
                ?? throw new InvalidOperationException($"Report definition '{ReportDefinition}' was not found as an embedded resource.");

            using var report = new LocalReport();
            report.LoadReportDefinition(definition);
            report.DataSources.Add(new ReportDataSource("AttendanceDataSet", rows));
            report.SetParameters(
            [
                new ReportParameter("Month", monthStart.ToString("yyyy-MM")),
                new ReportParameter("DepartmentName", departmentName)
            ]);

            var pdf = report.Render("PDF");
            return File(pdf, "application/pdf");
        }
    }
}
