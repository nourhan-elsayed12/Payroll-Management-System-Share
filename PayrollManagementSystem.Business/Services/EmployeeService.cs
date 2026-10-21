using Microsoft.EntityFrameworkCore;
using PayrollManagementSystem.Business.Common;
using PayrollManagementSystem.Business.Interfaces;
using PayrollManagementSystem.Domain.Entities;
using PayrollManagementSystem.DTOs.Common;
using PayrollManagementSystem.DTOs.Employees;

namespace PayrollManagementSystem.Business.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IAppDbContext _db;

        public EmployeeService(IAppDbContext db)
        {
            _db = db;
        }

        public async Task<PagedResult<EmployeeListItemDto>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            // Soft delete: deleted employees are never listed
            var query = _db.Employees.Where(e => !e.IsDeleted);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(e => e.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new EmployeeListItemDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    PhoneNumber = e.PhoneNumber,
                    Email = e.Email,
                    DepartmentName = e.Department.Name,
                    JobGradeName = e.JobGrade.Name,
                    HireDate = e.HireDate
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<EmployeeListItemDto>(items, page, pageSize, totalCount);
        }

        public Task<EmployeeItemDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return _db.Employees
                .Where(e => e.Id == id && !e.IsDeleted)
                .Select(e => new EmployeeItemDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    PhoneNumber = e.PhoneNumber,
                    Email = e.Email,
                    DepartmentName = e.Department.Name,
                    JobGradeName = e.JobGrade.Name
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public Task<EmployeeFormDto?> GetForEditAsync(int id, CancellationToken cancellationToken)
        {
            return _db.Employees
                .Where(e => e.Id == id && !e.IsDeleted)
                .Select(e => new EmployeeFormDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    BirthDate = e.BirthDate,
                    Address = e.Address,
                    PhoneNumber = e.PhoneNumber,
                    Email = e.Email,
                    HireDate = e.HireDate,
                    DepartmentId = e.DepartmentId,
                    JobGradeId = e.JobGradeId
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Result> CreateAsync(EmployeeFormDto dto, CancellationToken cancellationToken)
        {

            var error = await ValidateAsync(dto, excludeId: null, cancellationToken);
            if (error is not null)
            {
                return Result.Failure(error);
            }

            _db.Employees.Add(new Employee
            {
                Name = dto.Name,
                BirthDate = dto.BirthDate,
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                HireDate = dto.HireDate!.Value,
                DepartmentId = dto.DepartmentId!.Value,
                JobGradeId = dto.JobGradeId!.Value
            });
            await _db.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result> UpdateAsync(EmployeeFormDto dto, CancellationToken cancellationToken)
        {

            var error = await ValidateAsync(dto, excludeId: dto.Id, cancellationToken);
            if (error is not null)
            {
                return Result.Failure(error);
            }

            var hireDate = dto.HireDate!.Value;
            var departmentId = dto.DepartmentId!.Value;
            var jobGradeId = dto.JobGradeId!.Value;

            var affected = await _db.Employees
                .Where(e => e.Id == dto.Id && !e.IsDeleted)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(e => e.Name, dto.Name)
                    .SetProperty(e => e.BirthDate, dto.BirthDate)
                    .SetProperty(e => e.Address, dto.Address)
                    .SetProperty(e => e.PhoneNumber, dto.PhoneNumber)
                    .SetProperty(e => e.Email, dto.Email)
                    .SetProperty(e => e.HireDate, hireDate)
                    .SetProperty(e => e.DepartmentId, departmentId)
                    .SetProperty(e => e.JobGradeId, jobGradeId),
                    cancellationToken);

            return affected == 0 ? Result.Failure("الموظف غير موجود أو تم حذفه") : Result.Success();
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            // Soft delete: the row stays so that absence and payroll history keep their reference
            var affected = await _db.Employees
                .Where(e => e.Id == id && !e.IsDeleted)
                .ExecuteUpdateAsync(s => s.SetProperty(e => e.IsDeleted, true), cancellationToken);

            return affected == 0 ? Result.Failure("الموظف غير موجود أو تم حذفه") : Result.Success();
        }

        public async Task<IReadOnlyList<LookupItemDto>> GetDepartmentsLookupAsync(CancellationToken cancellationToken)
        {
            return await _db.Departments
                .OrderBy(d => d.Name)
                .Select(d => new LookupItemDto { Id = d.Id, Name = d.Name })
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<LookupItemDto>> GetJobGradesLookupAsync(CancellationToken cancellationToken)
        {
            return await _db.JobGrades
                .OrderBy(g => g.Id)
                .Select(g => new LookupItemDto { Id = g.Id, Name = g.Name })
                .ToListAsync(cancellationToken);
        }


        // Returns the first business-rule error, or null when the data is valid
        private async Task<string?> ValidateAsync(EmployeeFormDto dto, int? excludeId, CancellationToken cancellationToken)
        {
            if (!await _db.Departments.AnyAsync(d => d.Id == dto.DepartmentId, cancellationToken))
            {
                return "القسم المختار غير موجود";
            }

            if (!await _db.JobGrades.AnyAsync(g => g.Id == dto.JobGradeId, cancellationToken))
            {
                return "الدرجة الوظيفية المختارة غير موجودة";
            }

            if (dto.Email is not null && await _db.Employees.AnyAsync(
                    e => !e.IsDeleted && e.Email == dto.Email && (excludeId == null || e.Id != excludeId),
                    cancellationToken))
            {
                return "البريد الإلكتروني مسجل لموظف آخر";
            }

            if (dto.PhoneNumber is not null && await _db.Employees.AnyAsync(
                    e => !e.IsDeleted && e.PhoneNumber == dto.PhoneNumber && (excludeId == null || e.Id != excludeId),
                    cancellationToken))
            {
                return "رقم الهاتف مسجل لموظف آخر";
            }

            return null;
        }
    }
}
