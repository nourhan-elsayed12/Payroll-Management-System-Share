using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayrollManagementSystem.Domain.Entities;

namespace PayrollManagementSystem.DataAccess.Configurations
{
    public sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.Address)
                .HasMaxLength(250);

            builder.Property(e => e.PhoneNumber)
                .HasMaxLength(20);

            builder.Property(e => e.Email)
                .HasMaxLength(150);

            builder.Property(e => e.IsDeleted)
                .HasDefaultValue(false);

            builder.HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.JobGrade)
                .WithMany(g => g.Employees)
                .HasForeignKey(e => e.JobGradeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Non-unique (uniqueness is checked in EmployeeService.ValidateAsync).
            // Speeds up the "already used by another employee" checks on create/update.
            builder.HasIndex(e => e.Email);
            builder.HasIndex(e => e.PhoneNumber);
        }
    }
}
