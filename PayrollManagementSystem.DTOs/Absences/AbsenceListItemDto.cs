namespace PayrollManagementSystem.DTOs.Absences
{
    public class AbsenceListItemDto
    {
        public int Id { get; init; }

        public string EmployeeName { get; init; } = string.Empty;

        public DateOnly Date { get; init; }
    }
}
