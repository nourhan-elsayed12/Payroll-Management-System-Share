using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PayrollManagementSystem.Business.Interfaces;
using PayrollManagementSystem.DTOs.Employees;

namespace Payroll_Management_System.Controllers
{
    public sealed class EmployeesController : Controller
    {
        private const int PageSize = 10;

        private readonly IEmployeeService _employees;

        public EmployeesController(IEmployeeService employees)
        {
            _employees = employees;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, CancellationToken cancellationToken = default)
        {
            // ?page=0 or a negative page in the URL would make Skip() negative and fail in SQL
            if (page < 1)
            {
                page = 1;
            }

            var result = await _employees.GetPagedAsync(page, PageSize, cancellationToken);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            await LoadLookupsAsync(cancellationToken);
            return View(new EmployeeFormDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeFormDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                await LoadLookupsAsync(cancellationToken);
                return View(dto);
            }

            var result = await _employees.CreateAsync(dto, cancellationToken);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Error!);
                await LoadLookupsAsync(cancellationToken);
                return View(dto);
            }

            TempData["Success"] = "تمت إضافة الموظف بنجاح";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var dto = await _employees.GetForEditAsync(id, cancellationToken);
            if (dto is null)
            {
                return NotFound();
            }

            await LoadLookupsAsync(cancellationToken);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeFormDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                await LoadLookupsAsync(cancellationToken);
                return View(dto);
            }

            var result = await _employees.UpdateAsync(dto, cancellationToken);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Error!);
                await LoadLookupsAsync(cancellationToken);
                return View(dto);
            }

            TempData["Success"] = "تم تعديل الموظف بنجاح";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var item = await _employees.GetByIdAsync(id, cancellationToken);
            return item is null ? NotFound() : View(item);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
        {
            var result = await _employees.DeleteAsync(id, cancellationToken);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Error;
                return RedirectToAction("Index");
            }

            TempData["Success"] = "تم حذف الموظف بنجاح";
            return RedirectToAction("Index");
        }

        // Dropdown data for the employee form (departments + job grades)
        private async Task LoadLookupsAsync(CancellationToken cancellationToken)
        {
            var departments = await _employees.GetDepartmentsLookupAsync(cancellationToken);
            var jobGrades = await _employees.GetJobGradesLookupAsync(cancellationToken);

            ViewBag.Departments = new SelectList(departments, "Id", "Name");
            ViewBag.JobGrades = new SelectList(jobGrades, "Id", "Name");
        }
    }
}
