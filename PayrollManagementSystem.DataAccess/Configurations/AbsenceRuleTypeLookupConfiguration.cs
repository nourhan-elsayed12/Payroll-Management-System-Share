using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayrollManagementSystem.Domain.Entities;
using PayrollManagementSystem.Domain.Enums;

namespace PayrollManagementSystem.DataAccess.Configurations
{
    public sealed class AbsenceRuleTypeLookupConfiguration : IEntityTypeConfiguration<AbsenceRuleTypeLookup>
    {
        public void Configure(EntityTypeBuilder<AbsenceRuleTypeLookup> builder)
        {
            builder.ToTable("AbsenceRuleTypes");

            builder.HasKey(t => t.Id);

            // The Id is the enum value, not an IDENTITY column
            builder.Property(t => t.Id)
                .ValueGeneratedNever();

            builder.Property(t => t.Name)
                .HasMaxLength(20)
                .IsRequired();

            builder.HasData(
                new AbsenceRuleTypeLookup { Id = (int)AbsenceRuleType.Bonus, Name = "حافز" },
                new AbsenceRuleTypeLookup { Id = (int)AbsenceRuleType.Deduction, Name = "خصم" });
        }
    }
}
