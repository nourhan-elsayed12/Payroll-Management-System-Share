using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayrollManagementSystem.Domain.Entities;

namespace PayrollManagementSystem.DataAccess.Configurations
{
    public sealed class AbsenceConfiguration : IEntityTypeConfiguration<Absence>
    {
        public void Configure(EntityTypeBuilder<Absence> builder)
        {
            builder.ToTable("Absences");

            builder.HasKey(a => a.Id);

            builder.HasOne(a => a.Employee)
                .WithMany()
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(a => new { a.EmployeeId, a.Date });
            builder.HasIndex(a => a.Date);
        }
    }
}
