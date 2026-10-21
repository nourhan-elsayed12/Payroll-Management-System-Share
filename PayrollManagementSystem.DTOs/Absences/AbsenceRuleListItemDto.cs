namespace PayrollManagementSystem.DTOs.Absences
{
    public class AbsenceRuleListItemDto
    {
        public int Id { get; init; }

        public string Description { get; init; } = string.Empty;

        public string TypeName { get; init; } = string.Empty;


        public decimal Percentage { get; init; }

        public bool IsSet => Percentage > 0;
    }
}
