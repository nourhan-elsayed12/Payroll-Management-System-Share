namespace PayrollManagementSystem.DTOs.Incentives
{
    public class ServiceYearsIncentiveListItemDto
    {
        public int Id { get; init; }

        public int MinYears { get; init; }

        public decimal Percentage { get; init; }

        // A percentage of 0 means it has not been set yet
        public bool IsSet => Percentage > 0;
    }
}
