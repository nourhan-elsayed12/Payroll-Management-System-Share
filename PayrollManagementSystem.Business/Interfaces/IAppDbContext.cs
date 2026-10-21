using Microsoft.EntityFrameworkCore;
using PayrollManagementSystem.Domain.Entities;

namespace PayrollManagementSystem.Business.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<Employee> Employees { get; }

        DbSet<Department> Departments { get; }

        DbSet<JobGrade> JobGrades { get; }

        DbSet<ServiceYearsIncentive> ServiceYearsIncentives { get; }

        DbSet<Absence> Absences { get; }

        DbSet<AbsenceRule> AbsenceRules { get; }

        DbSet<AbsenceRuleTypeLookup> AbsenceRuleTypes { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
