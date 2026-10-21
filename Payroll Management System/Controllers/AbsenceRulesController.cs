using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PayrollManagementSystem.Business.Interfaces;
using PayrollManagementSystem.DTOs.Absences;

namespace Payroll_Management_System.Controllers
{
    public sealed class AbsenceRulesController : Controller
    {
        private const int PageSize = 10;

        private readonly IAbsenceRuleService _rules;

        public AbsenceRulesController(IAbsenceRuleService rules)
        {
            _rules = rules;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, CancellationToken cancellationToken = default)
        {
            // ?page=0 or a negative page in the URL would make Skip() negative and fail in SQL
            if (page < 1)
            {
                page = 1;
            }

            var result = await _rules.GetPagedAsync(page, PageSize, cancellationToken);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            await LoadTypesAsync(cancellationToken);
            return View(new AbsenceRuleFormDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AbsenceRuleFormDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                await LoadTypesAsync(cancellationToken);
                return View(dto);
            }

            var result = await _rules.CreateAsync(dto, cancellationToken);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Error!);
                await LoadTypesAsync(cancellationToken);
                return View(dto);
            }

            TempData["Success"] = "تمت إضافة الحالة بنجاح";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var dto = await _rules.GetForEditAsync(id, cancellationToken);
            if (dto is null)
            {
                return NotFound();
            }

            await LoadTypesAsync(cancellationToken);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AbsenceRuleFormDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                await LoadTypesAsync(cancellationToken);
                return View(dto);
            }

            var result = await _rules.UpdateAsync(dto, cancellationToken);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Error!);
                await LoadTypesAsync(cancellationToken);
                return View(dto);
            }

            TempData["Success"] = "تم تعديل الحالة بنجاح";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var item = await _rules.GetByIdAsync(id, cancellationToken);
            return item is null ? NotFound() : View(item);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
        {
            var result = await _rules.DeleteAsync(id, cancellationToken);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Error;
                return RedirectToAction("Index");
            }

            TempData["Success"] = "تم حذف الحالة بنجاح";
            return RedirectToAction("Index");
        }

        private async Task LoadTypesAsync(CancellationToken cancellationToken)
        {
            var types = await _rules.GetTypesLookupAsync(cancellationToken);
            ViewBag.Types = new SelectList(types, "Id", "Name");
        }
    }
}
