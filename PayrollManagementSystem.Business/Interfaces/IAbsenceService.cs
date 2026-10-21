using PayrollManagementSystem.Business.Common;
using PayrollManagementSystem.DTOs.Absences;
using PayrollManagementSystem.DTOs.Common;

namespace PayrollManagementSystem.Business.Interfaces
{
    public interface IAbsenceService
    {
        Task<PagedResult<AbsenceListItemDto>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken);

        Task<AbsenceItemDto?> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<Result> CreateAsync(AbsenceFormDto dto, CancellationToken cancellationToken);

        Task<Result> DeleteAsync(int id, CancellationToken cancellationToken);

        Task<List<LookupItemDto>> GetDepartmentsLookupAsync(CancellationToken cancellationToken);

        Task<List<LookupItemDto>> GetEmployeesLookupAsync(int departmentId, CancellationToken cancellationToken);
    }
}
