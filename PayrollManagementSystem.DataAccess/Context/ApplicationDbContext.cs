using Microsoft.EntityFrameworkCore;
using PayrollManagementSystem.Business.Interfaces;
using PayrollManagementSystem.Domain.Entities;

namespace PayrollManagementSystem.DataAccess.Context
{
    public class ApplicationDbContext : DbContext, IAppDbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<JobGrade> JobGrades => Set<JobGrade>();
        public DbSet<ServiceYearsIncentive> ServiceYearsIncentives => Set<ServiceYearsIncentive>();
        public DbSet<Absence> Absences => Set<Absence>();
        public DbSet<AbsenceRule> AbsenceRules => Set<AbsenceRule>();
        public DbSet<AbsenceRuleTypeLookup> AbsenceRuleTypes => Set<AbsenceRuleTypeLookup>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
