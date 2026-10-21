namespace PayrollManagementSystem.Domain.Entities
{
    /// <summary>
    /// Incentive based on years of service: employees with more than
    /// <see cref="MinYears"/> years get <see cref="Percentage"/> % of their salary.
    /// </summary>
    public class ServiceYearsIncentive
    {
        public int Id { get; set; }

        public int MinYears { get; set; }

        public decimal Percentage { get; set; }
    }
}
