using PayrollManagementSystem.Business.Common;
using PayrollManagementSystem.DTOs.Common;
using PayrollManagementSystem.DTOs.Incentives;

namespace PayrollManagementSystem.Business.Interfaces
{
    public interface IServiceYearsIncentiveService
    {
        Task<PagedResult<ServiceYearsIncentiveListItemDto>> GetPagedAsync(
            int page, int pageSize, CancellationToken cancellationToken);

        Task<ServiceYearsIncentiveItemDto?> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<ServiceYearsIncentiveFormDto?> GetForEditAsync(int id, CancellationToken cancellationToken);

        Task<Result> CreateAsync(ServiceYearsIncentiveFormDto dto, CancellationToken cancellationToken);

        Task<Result> UpdateAsync(ServiceYearsIncentiveFormDto dto, CancellationToken cancellationToken);

        Task<Result> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
