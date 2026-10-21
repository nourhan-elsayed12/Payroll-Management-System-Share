namespace PayrollManagementSystem.DTOs.Reports
{
    public class AttendanceReportRowDto
    {
        public string EmployeeName { get; init; } = string.Empty;

        public int AbsenceDays { get; init; }
    }
}
