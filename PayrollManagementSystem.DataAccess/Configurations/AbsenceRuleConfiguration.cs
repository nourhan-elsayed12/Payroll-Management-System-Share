using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayrollManagementSystem.Domain.Entities;
using PayrollManagementSystem.Domain.Enums;

namespace PayrollManagementSystem.DataAccess.Configurations
{
    public sealed class AbsenceRuleConfiguration : IEntityTypeConfiguration<AbsenceRule>
    {
        public void Configure(EntityTypeBuilder<AbsenceRule> builder)
        {
            builder.ToTable("AbsenceRules");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Percentage)
                .HasPrecision(5, 2);

            builder.HasOne(r => r.TypeInfo)
                .WithMany()
                .HasForeignKey(r => r.AbsenceRuleTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new AbsenceRule { Id = 1, FromDays = 0, ToDays = 1, AbsenceRuleTypeId = (int)AbsenceRuleType.Bonus, Percentage = 0 },
                new AbsenceRule { Id = 2, FromDays = 3, ToDays = 5, AbsenceRuleTypeId = (int)AbsenceRuleType.Deduction, Percentage = 0 },
                new AbsenceRule { Id = 3, FromDays = 6, ToDays = 10, AbsenceRuleTypeId = (int)AbsenceRuleType.Deduction, Percentage = 0 },
                new AbsenceRule { Id = 4, FromDays = 11, ToDays = 15, AbsenceRuleTypeId = (int)AbsenceRuleType.Deduction, Percentage = 0 },
                new AbsenceRule { Id = 5, FromDays = 16, ToDays = null, AbsenceRuleTypeId = (int)AbsenceRuleType.Deduction, Percentage = 0 });
        }
    }
}
