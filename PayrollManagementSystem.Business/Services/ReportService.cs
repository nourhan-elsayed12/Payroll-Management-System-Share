using Microsoft.EntityFrameworkCore;
using PayrollManagementSystem.Business.Interfaces;
using PayrollManagementSystem.Domain.Entities;
using PayrollManagementSystem.Domain.Enums;
using PayrollManagementSystem.DTOs.Common;
using PayrollManagementSystem.DTOs.Reports;

namespace PayrollManagementSystem.Business.Services
{
    public class ReportService : IReportService
    {
        private readonly IAppDbContext _db;

        public ReportService(IAppDbContext db)
        {
            _db = db;
        }

        public async Task<List<AttendanceReportRowDto>> GetAttendanceAsync(DateOnly month, int? departmentId, CancellationToken cancellationToken)
        {
            var from = month;
            var to = month.AddMonths(1);

            // Start from employees so those with no absences still appear with 0.
            // Deleted employees and those hired after the month are excluded.
            var query = _db.Employees.AsNoTracking()
                .Where(e => !e.IsDeleted && e.HireDate < to);

            if (departmentId is not null)
            {
                query = query.Where(e => e.DepartmentId == departmentId);
            }

            return await query
                .OrderBy(e => e.Name)
                .Select(e => new AttendanceReportRowDto
                {
                    EmployeeName = e.Name,
                    // Translated to a correlated subquery: one round trip to the database
                    AbsenceDays = _db.Absences.Count(a => a.EmployeeId == e.Id && a.Date >= from && a.Date < to)
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<EmployeesReportRowDto>> GetEmployeesAsync(int departmentId, CancellationToken cancellationToken)
        {
            // Deleted employees are not listed
            return await _db.Employees.AsNoTracking()
                .Where(e => !e.IsDeleted && e.DepartmentId == departmentId)
                .OrderBy(e => e.Name)
                .Select(e => new EmployeesReportRowDto
                {
                    EmployeeName = e.Name,
                    JobGradeName = e.JobGrade.Name,
                    PhoneNumber = e.PhoneNumber,
                    Email = e.Email,
                    HireDate = e.HireDate
                })
                .ToListAsync(cancellationToken);
        }



        public async Task<List<IncentivesReportRowDto>> GetIncentivesAsync(DateOnly month, int departmentId, CancellationToken cancellationToken)
        {
            var from = month;
            var to = month.AddMonths(1);
            var monthEnd = to.AddDays(-1);

            // One query: salary, department percentage, hire date and the month's absence count per employee.
            // Deleted employees and those hired after the month are excluded.
            var employees = await _db.Employees.AsNoTracking()
                .Where(e => !e.IsDeleted && e.DepartmentId == departmentId && e.HireDate < to)
                .OrderBy(e => e.Name)
                .Select(e => new
                {
                    e.Name,
                    e.HireDate,
                    e.JobGrade.BaseSalary,
                    DepartmentPercentage = e.Department.IncentivePercentage,
                    AbsenceDays = _db.Absences.Count(a => a.EmployeeId == e.Id && a.Date >= from && a.Date < to)
                })
                .ToListAsync(cancellationToken);

            // Small lookup tables: loaded once, then matched in memory for each employee
            var absenceRules = await _db.AbsenceRules.AsNoTracking().ToListAsync(cancellationToken);
            var serviceYearsIncentives = await _db.ServiceYearsIncentives.AsNoTracking()
                .OrderByDescending(t => t.MinYears)
                .ToListAsync(cancellationToken);
            var result = new List<IncentivesReportRowDto>();

            foreach (var e in employees)
            {
                // 1) حافز القسم
                var departmentIncentive = PercentOf(e.BaseSalary, e.DepartmentPercentage);

                // 2) حافز سنوات الخدمة
                var years = CompletedYears(e.HireDate, monthEnd);
                var serviceYearsPercentage = GetServiceYearsPercentage(years, serviceYearsIncentives);
                var yearsIncentive = PercentOf(e.BaseSalary, serviceYearsPercentage);

                // 3) الغياب: يا حافز يا خصم، حسب نوع القاعدة
                decimal absenceIncentive = 0;
                decimal absenceDeduction = 0;

                var rule = FindAbsenceRule(e.AbsenceDays, absenceRules);
                if (rule != null)
                {
                    var amount = PercentOf(e.BaseSalary, rule.Percentage);

                    if (rule.AbsenceRuleTypeId == (int)AbsenceRuleType.Bonus)
                        absenceIncentive = amount;
                    else
                        absenceDeduction = amount;
                }

                // 4) الصف
                result.Add(new IncentivesReportRowDto
                {
                    EmployeeName = e.Name,
                    BaseSalary = e.BaseSalary,
                    DepartmentIncentive = departmentIncentive,
                    ServiceYearsIncentive = yearsIncentive,
                    AbsenceIncentive = absenceIncentive,
                    AbsenceDeduction = absenceDeduction
                });
            }

            return result;

        }

        // نسبة أعلى شريحة الموظف وصلها (الشرايح مترتبة من الأكبر للأصغر)
        private static decimal GetServiceYearsPercentage(int years, List<ServiceYearsIncentive> incentives)
        {
            foreach (var item in incentives)
            {
                if (years >= item.MinYears)
                    return item.Percentage;   // أول شريحة يوصلها هي الأعلى
            }

            return 0;   // موصلش لأي شريحة
        }

        // القاعدة اللي عدد أيام الغياب واقع جواها، أو null لو مفيش
        private static AbsenceRule? FindAbsenceRule(int absenceDays, List<AbsenceRule> rules)
        {
            var rule = rules.FirstOrDefault(r =>
           absenceDays >= r.FromDays && (r.ToDays == null || absenceDays <= r.ToDays));
            return rule;
        }

        // Percentages are stored as 0-100; a 0 percentage ("not set") gives 0.
        // Keeps two decimals without rounding (12.349 -> 12.34)
        private static decimal PercentOf(decimal amount, decimal percentage)
        {
            return Math.Truncate(amount * percentage / 100 * 100) / 100;
        }

        // Full years of service from the hire date up to the given date
        private static int CompletedYears(DateOnly hireDate, DateOnly current)
        {
            var years = current.Year - hireDate.Year;
            if (hireDate.AddYears(years) > current)
            {
                years--;
            }

            return years;
        }

        public async Task<List<LookupItemDto>> GetDepartmentsLookupAsync(CancellationToken cancellationToken)
        {
            return await _db.Departments
                .OrderBy(d => d.Name)
                .Select(d => new LookupItemDto { Id = d.Id, Name = d.Name })
                .ToListAsync(cancellationToken);
        }

        public async Task<string?> GetDepartmentNameAsync(int departmentId,CancellationToken cancellationToken)
        {
            return await _db.Departments.Where(d => d.Id == departmentId).Select (d=>d.Name).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
