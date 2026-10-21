namespace PayrollManagementSystem.Domain.Enums
{
    /// <summary>
    /// Values must stay in sync with the AbsenceRuleTypes lookup table
    /// (it is seeded from this enum in AbsenceRuleTypeLookupConfiguration).
    /// Explicit numbers: never change them, they are stored in the database.
    /// </summary>
    public enum AbsenceRuleType
    {
        Bonus = 1,
        Deduction = 2
    }
}
