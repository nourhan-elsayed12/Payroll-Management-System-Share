using Microsoft.EntityFrameworkCore;
using PayrollManagementSystem.Business.Common;
using PayrollManagementSystem.Business.Interfaces;
using PayrollManagementSystem.Domain.Entities;
using PayrollManagementSystem.DTOs.Common;
using PayrollManagementSystem.DTOs.Departments;

namespace PayrollManagementSystem.Business.Services
{
    public class DepartmentService : IDepartmentService
    {

        private readonly IAppDbContext _db;

        public DepartmentService(IAppDbContext db)
        {
            _db = db;
        }

        public async Task<PagedResult<DepartmentListItemDto>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            var query = _db.Departments.AsNoTracking();

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(d => d.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new DepartmentListItemDto
                {
                    Id = d.Id,
                    Name = d.Name,
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<DepartmentListItemDto>(items, page, pageSize, totalCount);
        }

        public Task<DepartmentItemDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return _db.Departments
                .Where(d => d.Id == id)
                .Select(d => new DepartmentItemDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    EmployeesCount=d.Employees.Count()
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public Task<DepartmentFormDto?> GetForEditAsync(int id, CancellationToken cancellationToken)
        {
            return _db.Departments
                .Where(d => d.Id == id)
                .Select(d => new DepartmentFormDto
                {
                    Id = d.Id,
                    Name = d.Name
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Result> CreateAsync(DepartmentFormDto dto, CancellationToken cancellationToken)
        {
            var name = dto.Name.Trim();

            if (await NameExistsAsync(name, excludeId: null, cancellationToken))
            {
                return Result.Failure("يوجد قسم آخر بنفس الاسم");
            }

            _db.Departments.Add(new Department { Name = name });
            await _db.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result> UpdateAsync(DepartmentFormDto dto, CancellationToken cancellationToken)
        {
            var name = dto.Name.Trim();

            if (await NameExistsAsync(name, excludeId: dto.Id, cancellationToken))
            {
                return Result.Failure("يوجد قسم آخر بنفس الاسم");
            }

            var affected = await _db.Departments
                .Where(d => d.Id == dto.Id)
                .ExecuteUpdateAsync(s => s.SetProperty(d => d.Name, name), cancellationToken);

            return affected == 0 ? Result.Failure("القسم غير موجود أو تم حذفه") : Result.Success();
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var hasEmployees = await _db.Employees.AnyAsync(e => e.DepartmentId == id, cancellationToken);
            if (hasEmployees)
            {
                return Result.Failure("لا يمكن حذف القسم لوجود موظفين (حاليين أو سابقين) مسجلين عليه");
            }

            var affected = await _db.Departments
                .Where(d => d.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            return affected == 0 ? Result.Failure("القسم غير موجود أو تم حذفه") : Result.Success();
        }

        private Task<bool> NameExistsAsync(string name, int? excludeId, CancellationToken cancellationToken)
        {
            return _db.Departments.AnyAsync(
                d => d.Name == name && (excludeId == null || d.Id != excludeId),
                cancellationToken);
        }
    }
}
