
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayrollManagementSystem.Domain.Entities;

namespace PayrollManagementSystem.DataAccess.Configurations
{
    public sealed class ServiceYearsIncentiveConfiguration : IEntityTypeConfiguration<ServiceYearsIncentive>
    {
        public void Configure(EntityTypeBuilder<ServiceYearsIncentive> builder)
        {
            builder.ToTable("ServiceYearsIncentives");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Percentage)
                .HasPrecision(5, 2);

            builder.HasIndex(i => i.MinYears);

            builder.HasData(
                new ServiceYearsIncentive { Id = 1, MinYears = 5, Percentage = 0 },
                new ServiceYearsIncentive { Id = 2, MinYears = 7, Percentage = 0 });
        }
    }
}
