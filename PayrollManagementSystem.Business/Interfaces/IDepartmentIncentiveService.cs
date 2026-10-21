using PayrollManagementSystem.Business.Common;
using PayrollManagementSystem.DTOs.Incentives;

namespace PayrollManagementSystem.Business.Interfaces
{
    public interface IDepartmentIncentiveService
    {
        Task<List<DepartmentIncentiveListItemDto>> GetAllAsync(CancellationToken cancellationToken);

        Task<DepartmentIncentiveFormDto?> GetForEditAsync(int departmentId, CancellationToken cancellationToken);

        Task<Result> UpdateAsync(DepartmentIncentiveFormDto dto, CancellationToken cancellationToken);
    }
}
