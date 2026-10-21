using Microsoft.EntityFrameworkCore;
using PayrollManagementSystem.Business.Common;
using PayrollManagementSystem.Business.Interfaces;
using PayrollManagementSystem.Domain.Entities;
using PayrollManagementSystem.DTOs.Absences;
using PayrollManagementSystem.DTOs.Common;

namespace PayrollManagementSystem.Business.Services
{
    public class AbsenceService : IAbsenceService
    {
        private readonly IAppDbContext _db;

        public AbsenceService(IAppDbContext db)
        {
            _db = db;
        }

        public async Task<PagedResult<AbsenceListItemDto>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            // Absences of deleted employees are not listed
            var query = _db.Absences.AsNoTracking().Where(a => !a.Employee.IsDeleted);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(a => a.Date)
                .ThenByDescending(a => a.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new AbsenceListItemDto
                {
                    Id = a.Id,
                    EmployeeName = a.Employee.Name,
                    Date = a.Date
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<AbsenceListItemDto>(items, page, pageSize, totalCount);
        }

        public Task<AbsenceItemDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return _db.Absences
                .Where(a => a.Id == id)
                .Select(a => new AbsenceItemDto
                {
                    Id = a.Id,
                    EmployeeName = a.Employee.Name,
                    Date = a.Date
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Result> CreateAsync(AbsenceFormDto dto, CancellationToken cancellationToken)
        {
            var employeeId = dto.EmployeeId!.Value;
            var date = dto.Date!.Value;

            var employee = await _db.Employees
                .Where(e => e.Id == employeeId && !e.IsDeleted)
                .Select(e => new { e.HireDate, e.DepartmentId })
                .FirstOrDefaultAsync(cancellationToken);

            if (employee is null)
            {
                return Result.Failure("الموظف غير موجود");
            }

            // The dropdown alone is not enough: the request could be edited by hand
            if (employee.DepartmentId != dto.DepartmentId)
            {
                return Result.Failure("الموظف لا ينتمي للقسم المختار");
            }

            if (date < employee.HireDate)
            {
                return Result.Failure("لا يمكن تسجيل غياب قبل تاريخ تعيين الموظف");
            }

            if (await _db.Absences.AnyAsync(a => a.EmployeeId == employeeId && a.Date == date, cancellationToken))
            {
                return Result.Failure("تم تسجيل غياب لهذا الموظف في نفس اليوم بالفعل");
            }

            _db.Absences.Add(new Absence { EmployeeId = employeeId, Date = date });
            await _db.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var affected = await _db.Absences
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            return affected == 0 ? Result.Failure("سجل الغياب غير موجود أو تم حذفه") : Result.Success();
        }

        public async Task<List<LookupItemDto>> GetDepartmentsLookupAsync(CancellationToken cancellationToken)
        {
            return await _db.Departments
                .OrderBy(d => d.Name)
                .Select(d => new LookupItemDto { Id = d.Id, Name = d.Name })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<LookupItemDto>> GetEmployeesLookupAsync(int departmentId, CancellationToken cancellationToken)
        {
            return await _db.Employees
                .Where(e => e.DepartmentId == departmentId && !e.IsDeleted)
                .OrderBy(e => e.Name)
                .Select(e => new LookupItemDto { Id = e.Id, Name = e.Name })
                .ToListAsync(cancellationToken);
        }
    }
}
