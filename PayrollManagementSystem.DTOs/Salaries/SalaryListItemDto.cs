namespace PayrollManagementSystem.DTOs.Salaries
{
    public class SalaryListItemDto
    {
        public int JobGradeId { get; init; }

        public string JobGradeName { get; init; } = string.Empty;

        public decimal BaseSalary { get; init; }

    }
}
