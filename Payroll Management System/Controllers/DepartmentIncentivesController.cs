using Microsoft.AspNetCore.Mvc;
using PayrollManagementSystem.Business.Interfaces;
using PayrollManagementSystem.DTOs.Incentives;

namespace Payroll_Management_System.Controllers
{
    public sealed class DepartmentIncentivesController : Controller
    {
        private readonly IDepartmentIncentiveService _incentives;

        public DepartmentIncentivesController(IDepartmentIncentiveService incentives)
        {
            _incentives = incentives;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var result = await _incentives.GetAllAsync(cancellationToken);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var dto = await _incentives.GetForEditAsync(id, cancellationToken);
            return dto is null ? NotFound() : View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DepartmentIncentiveFormDto dto, CancellationToken cancellationToken)
        {
            dto.DepartmentId = id;

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var result = await _incentives.UpdateAsync(dto, cancellationToken);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Error!);
                return View(dto);
            }

            TempData["Success"] = "تم تعديل حافز القسم بنجاح";
            return RedirectToAction("Index");
        }
    }
}
