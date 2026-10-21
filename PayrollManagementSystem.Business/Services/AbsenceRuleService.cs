using Microsoft.EntityFrameworkCore;
using PayrollManagementSystem.Business.Common;
using PayrollManagementSystem.Business.Interfaces;
using PayrollManagementSystem.Domain.Entities;
using PayrollManagementSystem.Domain.Enums;
using PayrollManagementSystem.DTOs.Absences;
using PayrollManagementSystem.DTOs.Common;

namespace PayrollManagementSystem.Business.Services
{
    /// <summary>
    /// Monthly absence rules: a days range (FromDays..ToDays, ToDays null = no limit)
    /// with a bonus or deduction percentage. Ranges must not overlap, so every
    /// number of absence days matches one rule at most.
    /// </summary>
    public class AbsenceRuleService : IAbsenceRuleService
    {
        private readonly IAppDbContext _db;

        public AbsenceRuleService(IAppDbContext db)
        {
            _db = db;
        }

        public async Task<PagedResult<AbsenceRuleListItemDto>> GetPagedAsync(
            int page, int pageSize, CancellationToken cancellationToken)
        {
            var query = _db.AbsenceRules.AsNoTracking();

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
              .OrderBy(r => r.FromDays)
              .Skip((page - 1) * pageSize)
              .Take(pageSize)
              .Select(r => new AbsenceRuleListItemDto
              {
                  Id = r.Id,
                  Description = (r.ToDays==null)?"اكثر من "+r.FromDays:"من "+ r.FromDays+ "الى " +r.ToDays,
                  TypeName = r.TypeInfo.Name,
                  Percentage = r.Percentage
              })
              .ToListAsync(cancellationToken);

            return new PagedResult<AbsenceRuleListItemDto>(items, page, pageSize, totalCount);
        }

        public async Task<AbsenceRuleItemDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var rule = await _db.AbsenceRules
                .Where(r => r.Id == id)
                .Select(r => new
                {
                    r.Id,
                    r.FromDays,
                    r.ToDays,
                    TypeName = r.TypeInfo.Name,
                    r.Percentage
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (rule is null)
            {
                return null;
            }

            return new AbsenceRuleItemDto
            {
                Id = rule.Id,
                Description = (rule.ToDays == null) ? "اكثر من " + rule.FromDays : "من " + rule.FromDays + "الى " + rule.ToDays,
                TypeName = rule.TypeName,
                Percentage = rule.Percentage
            };
        }

        public Task<AbsenceRuleFormDto?> GetForEditAsync(int id, CancellationToken cancellationToken)
        {
            return _db.AbsenceRules
                .Where(r => r.Id == id)
                .Select(r => new AbsenceRuleFormDto
                {
                    Id = r.Id,
                    FromDays = r.FromDays,
                    ToDays = r.ToDays,
                    AbsenceRuleTypeId = r.AbsenceRuleTypeId,
                    Percentage = r.Percentage > 0 ? r.Percentage : null
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Result> CreateAsync(AbsenceRuleFormDto dto, CancellationToken cancellationToken)
        {
            var error = await ValidateAsync(dto, excludeId: null, cancellationToken);
            if (error is not null)
            {
                return Result.Failure(error);
            }

            _db.AbsenceRules.Add(new AbsenceRule
            {
                FromDays = dto.FromDays!.Value,
                ToDays = dto.ToDays,
                AbsenceRuleTypeId = dto.AbsenceRuleTypeId!.Value,
                Percentage = dto.Percentage!.Value
            });
            await _db.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result> UpdateAsync(AbsenceRuleFormDto dto, CancellationToken cancellationToken)
        {
            var error = await ValidateAsync(dto, excludeId: dto.Id, cancellationToken);
            if (error is not null)
            {
                return Result.Failure(error);
            }

            var fromDays = dto.FromDays!.Value;
            var toDays = dto.ToDays;
            var typeId = dto.AbsenceRuleTypeId!.Value;
            var percentage = dto.Percentage!.Value;

            var affected = await _db.AbsenceRules
                .Where(r => r.Id == dto.Id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(r => r.FromDays, fromDays)
                    .SetProperty(r => r.ToDays, toDays)
                    .SetProperty(r => r.AbsenceRuleTypeId, typeId)
                    .SetProperty(r => r.Percentage, percentage),
                    cancellationToken);

            return affected == 0 ? Result.Failure("الحالة غير موجودة أو تم حذفها") : Result.Success();
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var affected = await _db.AbsenceRules
                .Where(r => r.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            return affected == 0 ? Result.Failure("الحالة غير موجودة أو تم حذفها") : Result.Success();
        }

        public async Task<List<LookupItemDto>> GetTypesLookupAsync(CancellationToken cancellationToken)
        {
            return await _db.AbsenceRuleTypes
                .OrderBy(t => t.Id)
                .Select(t => new LookupItemDto { Id = t.Id, Name = t.Name })
                .ToListAsync(cancellationToken);
        }

        // Returns the first business-rule error, or null when the data is valid
        private async Task<string?> ValidateAsync(AbsenceRuleFormDto dto, int? excludeId, CancellationToken cancellationToken)
        {
            var fromDays = dto.FromDays!.Value;
            var toDays = dto.ToDays;

            if (toDays is not null && toDays < fromDays)
            {
                return "نهاية المدى يجب أن تكون أكبر من أو تساوي بدايته";
            }

            if (!await _db.AbsenceRuleTypes.AnyAsync(t => t.Id == dto.AbsenceRuleTypeId, cancellationToken))
            {
                return "النوع المختار غير موجود";
            }

            // Two ranges overlap when each one starts before the other one ends
            var upper = toDays ?? int.MaxValue;
            var overlaps = await _db.AbsenceRules.AnyAsync(
                r => (excludeId == null || r.Id != excludeId)
                     && r.FromDays <= upper
                     && (r.ToDays == null || r.ToDays >= fromDays),
                cancellationToken);

            if (overlaps)
            {
                return "هذا المدى يتداخل مع حالة أخرى موجودة";
            }

            return null;
        }



    }
}
