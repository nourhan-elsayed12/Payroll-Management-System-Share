using Microsoft.AspNetCore.Mvc;
using PayrollManagementSystem.Business.Common;
using PayrollManagementSystem.Business.Interfaces;
using PayrollManagementSystem.DTOs.Departments;

namespace Payroll_Management_System.Controllers
{
    public sealed class DepartmentsController : Controller
    {
        private const int PageSize = 10;

        private readonly IDepartmentService _departments;

        public DepartmentsController(IDepartmentService departments)
        {
            _departments = departments;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, CancellationToken cancellationToken = default)
        {
            // ?page=0 or a negative page in the URL would make Skip() negative and fail in SQL
            if (page < 1)
            {
                page = 1;
            }

            var result = await _departments.GetPagedAsync(page, PageSize, cancellationToken);
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new DepartmentFormDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentFormDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var result = await _departments.CreateAsync(dto, cancellationToken);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(nameof(dto.Name), result.Error!);
                return View(dto);
            }

            TempData["Success"] = "تمت إضافة القسم بنجاح";
            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var dto = await _departments.GetForEditAsync(id, cancellationToken);
            return dto is null ? NotFound() : View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( DepartmentFormDto dto, CancellationToken cancellationToken)
        {

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var result = await _departments.UpdateAsync(dto, cancellationToken);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(nameof(dto.Name), result.Error!);
                return View(dto);
            }

            TempData["Success"] = "تم تعديل القسم بنجاح";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var item = await _departments.GetByIdAsync(id, cancellationToken);
            return item is null ? NotFound() : View(item);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
        {
            var result = await _departments.DeleteAsync(id, cancellationToken);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Error;
                return RedirectToAction("Index");
            }

            TempData["Success"] = "تم حذف القسم بنجاح";
            return RedirectToAction("Index");
        }

    }
}
