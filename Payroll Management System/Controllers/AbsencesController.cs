using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PayrollManagementSystem.Business.Interfaces;
using PayrollManagementSystem.DTOs.Absences;
using PayrollManagementSystem.DTOs.Common;

namespace Payroll_Management_System.Controllers
{
    public sealed class AbsencesController : Controller
    {
        private const int PageSize = 10;

        private readonly IAbsenceService _absences;

        public AbsencesController(IAbsenceService absences)
        {
            _absences = absences;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, CancellationToken cancellationToken = default)
        {
            // ?page=0 or a negative page in the URL would make Skip() negative and fail in SQL
            if (page < 1)
            {
                page = 1;
            }

            var result = await _absences.GetPagedAsync(page, PageSize, cancellationToken);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            await LoadLookupsAsync(null, cancellationToken);
            return View(new AbsenceFormDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AbsenceFormDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                await LoadLookupsAsync(dto.DepartmentId, cancellationToken);
                return View(dto);
            }

            var result = await _absences.CreateAsync(dto, cancellationToken);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Error!);
                await LoadLookupsAsync(dto.DepartmentId, cancellationToken);
                return View(dto);
            }

            TempData["Success"] = "تم تسجيل الغياب بنجاح";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var item = await _absences.GetByIdAsync(id, cancellationToken);
            return item is null ? NotFound() : View(item);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
        {
            var result = await _absences.DeleteAsync(id, cancellationToken);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Error;
                return RedirectToAction("Index");
            }

            TempData["Success"] = "تم حذف سجل الغياب بنجاح";
            return RedirectToAction("Index");
        }

        // Called by employees-by-department.js when the department changes
        [HttpGet]
        public async Task<IActionResult> EmployeesByDepartment(int departmentId, CancellationToken cancellationToken)
        {
            var employees = await _absences.GetEmployeesLookupAsync(departmentId, cancellationToken);
            return Json(employees);
        }

        // Departments dropdown + employees of the selected department (empty until one is chosen)
        private async Task LoadLookupsAsync(int? departmentId, CancellationToken cancellationToken)
        {
            var departments = await _absences.GetDepartmentsLookupAsync(cancellationToken);
            ViewBag.Departments = new SelectList(departments, "Id", "Name");

            var employees = departmentId is null
                ? new List<LookupItemDto>()
                : await _absences.GetEmployeesLookupAsync(departmentId.Value, cancellationToken);
            ViewBag.Employees = new SelectList(employees, "Id", "Name");
        }
    }
}
