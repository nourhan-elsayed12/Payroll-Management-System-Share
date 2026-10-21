using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Reporting.NETCore;
using Payroll_Management_System.Helpers;
using PayrollManagementSystem.Business.Interfaces;

namespace Payroll_Management_System.Controllers
{
    // Incentives and deductions of a department for the last completed month,
    // rendered from Reports/Incentives.rdlc as PDF and shown inside the page in an <iframe>
    public sealed class IncentivesReportController : Controller
    {
        // Embedded resource name = RootNamespace + folder + file name
        private const string ReportDefinition = "Payroll_Management_System.Reports.Incentives.rdlc";

        private readonly IReportService _reports;

        public IncentivesReportController(IReportService reports)
        {
            _reports = reports;
        }

        // Filter page; the report frame appears once a department is chosen
        [HttpGet]
        public async Task<IActionResult> Index(int? departmentId, CancellationToken cancellationToken)
        {
            var departments = await _reports.GetDepartmentsLookupAsync(cancellationToken);
            ViewBag.Departments = new SelectList(departments, "Id", "Name", departmentId);
            ViewData["DepartmentId"] = departmentId;
            ViewData["Month"] = ReportMonth.Previous().ToString("yyyy-MM");

            return View();
        }

        // Returns the PDF inline (no file name) so the browser displays it instead of downloading
        [HttpGet]
        public async Task<IActionResult> Render(int departmentId, CancellationToken cancellationToken)
        {
            var departmentName = await _reports.GetDepartmentNameAsync(departmentId, cancellationToken);
            if (departmentName is null)
            {
                return NotFound();
            }

            var month = ReportMonth.Previous();
            var rows = await _reports.GetIncentivesAsync(month, departmentId, cancellationToken);

            using var definition = typeof(IncentivesReportController).Assembly.GetManifestResourceStream(ReportDefinition)
                ?? throw new InvalidOperationException($"Report definition '{ReportDefinition}' was not found as an embedded resource.");

            using var report = new LocalReport();
            report.LoadReportDefinition(definition);
            report.DataSources.Add(new ReportDataSource("IncentivesDataSet", rows));
            report.SetParameters(
            [
                new ReportParameter("Month", month.ToString("yyyy-MM")),
                new ReportParameter("DepartmentName", departmentName)
            ]);

            var pdf = report.Render("PDF");
            return File(pdf, "application/pdf");
        }
    }
}
