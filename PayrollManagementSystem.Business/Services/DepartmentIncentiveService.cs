using Microsoft.EntityFrameworkCore;
using PayrollManagementSystem.Business.Common;
using PayrollManagementSystem.Business.Interfaces;
using PayrollManagementSystem.DTOs.Incentives;

namespace PayrollManagementSystem.Business.Services
{
    /// <summary>
    /// Department incentive is stored on the department (Department.IncentivePercentage).
    /// A percentage of 0 means the incentive of that department has not been set yet.
    /// </summary>
    public class DepartmentIncentiveService : IDepartmentIncentiveService
    {
        private readonly IAppDbContext _db;

        public DepartmentIncentiveService(IAppDbContext db)
        {
            _db = db;
        }

        public async Task<List<DepartmentIncentiveListItemDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _db.Departments
                .OrderBy(d => d.Name)
                .Select(d => new DepartmentIncentiveListItemDto
                {
                    DepartmentId = d.Id,
                    DepartmentName = d.Name,
                    Percentage = d.IncentivePercentage
                })
                .ToListAsync(cancellationToken);
        }

        public Task<DepartmentIncentiveFormDto?> GetForEditAsync(int departmentId, CancellationToken cancellationToken)
        {
            return _db.Departments
                .Where(d => d.Id == departmentId)
                .Select(d => new DepartmentIncentiveFormDto
                {
                    DepartmentId = d.Id,
                    DepartmentName = d.Name,
                    Percentage = d.IncentivePercentage > 0 ? d.IncentivePercentage : null
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Result> UpdateAsync(DepartmentIncentiveFormDto dto, CancellationToken cancellationToken)
        {
            var departmentId = dto.DepartmentId!.Value;
            var percentage = dto.Percentage!.Value;

            var affected = await _db.Departments
                .Where(d => d.Id == departmentId)
                .ExecuteUpdateAsync(s => s.SetProperty(d => d.IncentivePercentage, percentage), cancellationToken);

            return affected == 0
                ? Result.Failure("القسم غير موجود")
                : Result.Success();
        }
    }
}
