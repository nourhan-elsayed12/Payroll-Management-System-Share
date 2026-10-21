using PayrollManagementSystem.Business.Common;
using PayrollManagementSystem.DTOs.Common;
using PayrollManagementSystem.DTOs.Salaries;

namespace PayrollManagementSystem.Business.Interfaces
{
    public interface ISalaryService
    {
        Task<List<SalaryListItemDto>> GetAllAsync(CancellationToken cancellationToken);

        Task<SalaryFormDto?> GetForEditAsync(int jobGradeId, CancellationToken cancellationToken);


        Task<Result> UpdateAsync(SalaryFormDto dto, CancellationToken cancellationToken);

    }
}
