using PayrollManagementSystem.Business.Common;
using PayrollManagementSystem.DTOs.Absences;
using PayrollManagementSystem.DTOs.Common;

namespace PayrollManagementSystem.Business.Interfaces
{
    public interface IAbsenceRuleService
    {
        Task<PagedResult<AbsenceRuleListItemDto>> GetPagedAsync(
            int page, int pageSize, CancellationToken cancellationToken);

        Task<AbsenceRuleItemDto?> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<AbsenceRuleFormDto?> GetForEditAsync(int id, CancellationToken cancellationToken);

        Task<Result> CreateAsync(AbsenceRuleFormDto dto, CancellationToken cancellationToken);

        Task<Result> UpdateAsync(AbsenceRuleFormDto dto, CancellationToken cancellationToken);

        Task<Result> DeleteAsync(int id, CancellationToken cancellationToken);

        // Types dropdown (from the AbsenceRuleTypes lookup table)
        Task<List<LookupItemDto>> GetTypesLookupAsync(CancellationToken cancellationToken);
    }
}
