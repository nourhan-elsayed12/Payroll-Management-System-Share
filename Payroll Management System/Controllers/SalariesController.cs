using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PayrollManagementSystem.Business.Interfaces;
using PayrollManagementSystem.DTOs.Salaries;

namespace Payroll_Management_System.Controllers
{
    public sealed class SalariesController : Controller
    {
        private readonly ISalaryService _salaries;

        public SalariesController(ISalaryService salaries)
        {
            _salaries = salaries;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var result = await _salaries.GetAllAsync(cancellationToken);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var dto = await _salaries.GetForEditAsync(id, cancellationToken);
            return dto is null ? NotFound() : View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SalaryFormDto dto, CancellationToken cancellationToken)
        {
            dto.JobGradeId = id;

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var result = await _salaries.UpdateAsync(dto, cancellationToken);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Error!);
                return View(dto);
            }

            TempData["Success"] = "تم تعديل المرتب بنجاح";
            return RedirectToAction("Index");
        }

    }
}
