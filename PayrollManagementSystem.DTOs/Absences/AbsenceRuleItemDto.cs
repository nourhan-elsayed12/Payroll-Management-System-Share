namespace PayrollManagementSystem.DTOs.Absences
{
    public class AbsenceRuleItemDto
    {
        public int Id { get; init; }

        public string Description { get; init; } = string.Empty;

        public string TypeName { get; init; } = string.Empty;

        public decimal Percentage { get; init; }
    }
}
