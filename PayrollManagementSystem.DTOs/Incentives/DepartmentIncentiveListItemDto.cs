namespace PayrollManagementSystem.DTOs.Incentives
{
    public class DepartmentIncentiveListItemDto
    {
        public int DepartmentId { get; init; }

        public string DepartmentName { get; init; } = string.Empty;

        public decimal Percentage { get; init; }

        public bool IsSet => Percentage > 0;
    }
}
