using PayrollManagementSystem.Business.Common;
using PayrollManagementSystem.DTOs.Common;
using PayrollManagementSystem.DTOs.Employees;

namespace PayrollManagementSystem.Business.Interfaces
{
    public interface IEmployeeService
    {
        Task<PagedResult<EmployeeListItemDto>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken);

        Task<EmployeeItemDto?> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<EmployeeFormDto?> GetForEditAsync(int id, CancellationToken cancellationToken);

        Task<Result> CreateAsync(EmployeeFormDto dto, CancellationToken cancellationToken);

        Task<Result> UpdateAsync(EmployeeFormDto dto, CancellationToken cancellationToken);

        Task<Result> DeleteAsync(int id, CancellationToken cancellationToken);

        Task<IReadOnlyList<LookupItemDto>> GetDepartmentsLookupAsync(CancellationToken cancellationToken);

        Task<IReadOnlyList<LookupItemDto>> GetJobGradesLookupAsync(CancellationToken cancellationToken);
    }
}
