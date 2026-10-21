using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayrollManagementSystem.Domain.Entities;

namespace PayrollManagementSystem.DataAccess.Configurations
{
    public sealed class JobGradeConfiguration : IEntityTypeConfiguration<JobGrade>
    {
        public void Configure(EntityTypeBuilder<JobGrade> builder)
        {
            builder.ToTable("JobGrades");

            builder.HasKey(g => g.Id);

            builder.Property(g => g.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(g => g.BaseSalary)
                .HasPrecision(18, 2);

            // Fixed grades 
            builder.HasData(
                new JobGrade { Id = 1, Name = "الدرجة الأولى", BaseSalary = 0 },
                new JobGrade { Id = 2, Name = "الدرجة الثانية", BaseSalary = 0 },
                new JobGrade { Id = 3, Name = "الدرجة الثالثة", BaseSalary = 0 });
        }
    }
}
