using Microsoft.EntityFrameworkCore;
using PayrollManagementSystem.Business.Common;
using PayrollManagementSystem.Business.Interfaces;
using PayrollManagementSystem.Domain.Entities;
using PayrollManagementSystem.DTOs.Common;
using PayrollManagementSystem.DTOs.Incentives;

namespace PayrollManagementSystem.Business.Services
{
    /// <summary>
    /// Years-of-service incentive tiers: employees with more than MinYears
    /// years of service get Percentage % of their base salary.
    /// </summary>
    public class ServiceYearsIncentiveService : IServiceYearsIncentiveService
    {
        private readonly IAppDbContext _db;

        public ServiceYearsIncentiveService(IAppDbContext db)
        {
            _db = db;
        }

        public async Task<PagedResult<ServiceYearsIncentiveListItemDto>> GetPagedAsync(
            int page, int pageSize, CancellationToken cancellationToken)
        {
            var query = _db.ServiceYearsIncentives.AsNoTracking();

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(i => i.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(i => new ServiceYearsIncentiveListItemDto
                {
                    Id = i.Id,
                    MinYears = i.MinYears,
                    Percentage = i.Percentage
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<ServiceYearsIncentiveListItemDto>(items, page, pageSize, totalCount);
        }

        public Task<ServiceYearsIncentiveItemDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return _db.ServiceYearsIncentives
                .Where(i => i.Id == id)
                .Select(i => new ServiceYearsIncentiveItemDto
                {
                    Id = i.Id,
                    MinYears = i.MinYears,
                    Percentage = i.Percentage
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public Task<ServiceYearsIncentiveFormDto?> GetForEditAsync(int id, CancellationToken cancellationToken)
        {
            return _db.ServiceYearsIncentives
                .Where(i => i.Id == id)
                .Select(i => new ServiceYearsIncentiveFormDto
                {
                    Id = i.Id,
                    MinYears = i.MinYears,
                    // Not set yet (0) is shown as an empty field
                    Percentage = i.Percentage > 0 ? i.Percentage : null
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Result> CreateAsync(ServiceYearsIncentiveFormDto dto, CancellationToken cancellationToken)
        {
            var minYears = dto.MinYears!.Value;

            if (await MinYearsExistsAsync(minYears, excludeId: null, cancellationToken))
            {
                return Result.Failure("توجد شريحة أخرى بنفس عدد السنوات");
            }

            _db.ServiceYearsIncentives.Add(new ServiceYearsIncentive
            {
                MinYears = minYears,
                Percentage = dto.Percentage!.Value
            });
            await _db.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result> UpdateAsync(ServiceYearsIncentiveFormDto dto, CancellationToken cancellationToken)
        {
            var minYears = dto.MinYears!.Value;
            var percentage = dto.Percentage!.Value;

            if (await MinYearsExistsAsync(minYears, excludeId: dto.Id, cancellationToken))
            {
                return Result.Failure("توجد شريحة أخرى بنفس عدد السنوات");
            }

            var affected = await _db.ServiceYearsIncentives
                .Where(i => i.Id == dto.Id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(i => i.MinYears, minYears)
                    .SetProperty(i => i.Percentage, percentage),
                    cancellationToken);

            return affected == 0 ? Result.Failure("الشريحة غير موجودة أو تم حذفها") : Result.Success();
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var affected = await _db.ServiceYearsIncentives
                .Where(i => i.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            return affected == 0 ? Result.Failure("الشريحة غير موجودة أو تم حذفها") : Result.Success();
        }

        private Task<bool> MinYearsExistsAsync(int minYears, int? excludeId, CancellationToken cancellationToken)
        {
            return _db.ServiceYearsIncentives.AnyAsync(
                i => i.MinYears == minYears && (excludeId == null || i.Id != excludeId),
                cancellationToken);
        }
    }
}
