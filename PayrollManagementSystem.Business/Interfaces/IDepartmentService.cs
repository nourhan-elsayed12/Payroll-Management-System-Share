using PayrollManagementSystem.Business.Common;
using PayrollManagementSystem.DTOs.Common;
using PayrollManagementSystem.DTOs.Departments;

namespace PayrollManagementSystem.Business.Interfaces
{
    public interface IDepartmentService
    {
        Task<PagedResult<DepartmentListItemDto>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken);

        Task<DepartmentItemDto?> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<DepartmentFormDto?> GetForEditAsync(int id, CancellationToken cancellationToken);

        Task<Result> CreateAsync(DepartmentFormDto dto, CancellationToken cancellationToken);

        Task<Result> UpdateAsync(DepartmentFormDto dto, CancellationToken cancellationToken);

        Task<Result> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
