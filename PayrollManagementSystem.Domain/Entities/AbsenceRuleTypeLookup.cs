namespace PayrollManagementSystem.Domain.Entities
{
    /// <summary>
    /// Lookup table for absence rule types (protects the data with a FK).
    /// Ids match the AbsenceRuleType enum values.
    /// </summary>
    public class AbsenceRuleTypeLookup
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
