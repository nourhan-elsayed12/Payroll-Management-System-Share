namespace PayrollManagementSystem.DTOs.Employees
{
    public class EmployeeItemDto
    {
        public int Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public string? PhoneNumber { get; init; }

        public string? Email { get; init; }

        public string DepartmentName { get; init; } = string.Empty;

        public string JobGradeName { get; init; } = string.Empty;
    }
}
