using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Payroll_Management_System.Helpers;
using PayrollManagementSystem.Business.Interfaces;

namespace Payroll_Management_System.Controllers
{
    // Attendance report rendered as an HTML page (with browser printing)
    public sealed class AttendanceReportController : Controller
    {
        private readonly IReportService _reports;

        public AttendanceReportController(IReportService reports)
        {
            _reports = reports;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? month, int? departmentId, CancellationToken cancellationToken)
        {
            var monthStart = ReportMonth.Parse(month);

            var rows = await _reports.GetAttendanceAsync(monthStart, departmentId, cancellationToken);

            var departments = await _reports.GetDepartmentsLookupAsync(cancellationToken);
            ViewBag.Departments = new SelectList(departments, "Id", "Name", departmentId);
            ViewData["Month"] = monthStart.ToString("yyyy-MM");
            ViewData["MaxMonth"] = ReportMonth.Current().ToString("yyyy-MM");

            return View(rows);
        }
    }
}
