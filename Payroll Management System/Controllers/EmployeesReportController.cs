using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Reporting.NETCore;
using PayrollManagementSystem.Business.Interfaces;

namespace Payroll_Management_System.Controllers
{
    // Company employees report rendered from Reports/Employees.rdlc as PDF,
    // shown inside the page in an <iframe>
    public sealed class EmployeesReportController : Controller
    {
        // Embedded resource name = RootNamespace + folder + file name
        private const string ReportDefinition = "Payroll_Management_System.Reports.Employees.rdlc";

        private readonly IReportService _reports;

        public EmployeesReportController(IReportService reports)
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

            var rows = await _reports.GetEmployeesAsync(departmentId, cancellationToken);

            using var definition = typeof(EmployeesReportController).Assembly.GetManifestResourceStream(ReportDefinition)
                ?? throw new InvalidOperationException($"Report definition '{ReportDefinition}' was not found as an embedded resource.");

            using var report = new LocalReport();
            report.LoadReportDefinition(definition);
            report.DataSources.Add(new ReportDataSource("EmployeesDataSet", rows));
            report.SetParameters([new ReportParameter("DepartmentName", departmentName)]);

            var pdf = report.Render("PDF");
            return File(pdf, "application/pdf");
        }
    }
}
