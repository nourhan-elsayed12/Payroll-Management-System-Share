namespace PayrollManagementSystem.DTOs.Absences
{
    public class AbsenceItemDto
    {
        public int Id { get; init; }

        public string EmployeeName { get; init; } = string.Empty;

        public DateOnly Date { get; init; }
    }
}
