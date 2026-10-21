namespace PayrollManagementSystem.DTOs.Reports
{
    public class EmployeesReportRowDto
    {
        public string EmployeeName { get; init; } = string.Empty;

        public string JobGradeName { get; init; } = string.Empty;

        public string? PhoneNumber { get; init; }

        public string? Email { get; init; }

        public DateOnly HireDate { get; init; }

        // RDLC has no DateOnly type, so the report binds to this text field
        public string HireDateText => HireDate.ToString("yyyy-MM-dd");
    }
}
