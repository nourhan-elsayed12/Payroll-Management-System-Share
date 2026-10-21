using Microsoft.EntityFrameworkCore;
using PayrollManagementSystem.Business.Common;
using PayrollManagementSystem.Business.Interfaces;
using PayrollManagementSystem.DTOs.Common;
using PayrollManagementSystem.DTOs.Salaries;

namespace PayrollManagementSystem.Business.Services
{
    /// <summary>
    /// Salaries are stored per job grade (JobGrade.BaseSalary).
    /// A BaseSalary of 0 means the salary of that grade has not been set yet.
    /// </summary>
    public class SalaryService : ISalaryService
    {
        private readonly IAppDbContext _db;

        public SalaryService(IAppDbContext db)
        {
            _db = db;
        }

        public async Task<List<SalaryListItemDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _db.JobGrades
                .OrderBy(g => g.Id)
                .Select(g => new SalaryListItemDto
                {
                    JobGradeId = g.Id,
                    JobGradeName = g.Name,
                    BaseSalary = g.BaseSalary,
                })
                .ToListAsync(cancellationToken);
        }

        public Task<SalaryFormDto?> GetForEditAsync(int jobGradeId, CancellationToken cancellationToken)
        {
            return _db.JobGrades
                .Where(g => g.Id == jobGradeId)
                .Select(g => new SalaryFormDto
                {
                    JobGradeId = g.Id,
                    JobGradeName = g.Name,
                    BaseSalary = g.BaseSalary
                })
                .FirstOrDefaultAsync(cancellationToken);
        }


        public async Task<Result> UpdateAsync(SalaryFormDto dto, CancellationToken cancellationToken)
        {
            var jobGradeId = dto.JobGradeId!.Value;
            var baseSalary = dto.BaseSalary!.Value;

            var affected = await _db.JobGrades
                .Where(g => g.Id == jobGradeId )
                .ExecuteUpdateAsync(s => s.SetProperty(g => g.BaseSalary, baseSalary), cancellationToken);

            return affected == 0
                ? Result.Failure("الدرجة الوظيفية غير موجودة ")
                : Result.Success();
        }

    }
}
