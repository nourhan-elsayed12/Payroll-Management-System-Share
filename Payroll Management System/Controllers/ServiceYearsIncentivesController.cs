using Microsoft.AspNetCore.Mvc;
using PayrollManagementSystem.Business.Interfaces;
using PayrollManagementSystem.DTOs.Incentives;

namespace Payroll_Management_System.Controllers
{
    public sealed class ServiceYearsIncentivesController : Controller
    {
        private const int PageSize = 10;

        private readonly IServiceYearsIncentiveService _incentives;

        public ServiceYearsIncentivesController(IServiceYearsIncentiveService incentives)
        {
            _incentives = incentives;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, CancellationToken cancellationToken = default)
        {
            // ?page=0 or a negative page in the URL would make Skip() negative and fail in SQL
            if (page < 1)
            {
                page = 1;
            }

            var result = await _incentives.GetPagedAsync(page, PageSize, cancellationToken);
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new ServiceYearsIncentiveFormDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceYearsIncentiveFormDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var result = await _incentives.CreateAsync(dto, cancellationToken);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(nameof(dto.MinYears), result.Error!);
                return View(dto);
            }

            TempData["Success"] = "تمت إضافة الشريحة بنجاح";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var dto = await _incentives.GetForEditAsync(id, cancellationToken);
            return dto is null ? NotFound() : View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ServiceYearsIncentiveFormDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var result = await _incentives.UpdateAsync(dto, cancellationToken);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(nameof(dto.MinYears), result.Error!);
                return View(dto);
            }

            TempData["Success"] = "تم تعديل الشريحة بنجاح";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var item = await _incentives.GetByIdAsync(id, cancellationToken);
            return item is null ? NotFound() : View(item);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
        {
            var result = await _incentives.DeleteAsync(id, cancellationToken);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Error;
                return RedirectToAction("Index");
            }

            TempData["Success"] = "تم حذف الشريحة بنجاح";
            return RedirectToAction("Index");
        }
    }
}
