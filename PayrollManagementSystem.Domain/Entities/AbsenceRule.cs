namespace PayrollManagementSystem.Domain.Entities
{
    /// <summary>
    /// Monthly absence rule: when the number of absence days in a month is between
    /// <see cref="FromDays"/> and <see cref="ToDays"/> (null = no upper limit),
    /// the salary gets a bonus or a deduction of <see cref="Percentage"/> %.
    /// </summary>
    public class AbsenceRule
    {
        public int Id { get; set; }

        public int FromDays { get; set; }

        public int? ToDays { get; set; }

        public int AbsenceRuleTypeId { get; set; }

        public AbsenceRuleTypeLookup TypeInfo { get; set; } = null!;

        public decimal Percentage { get; set; }
    }
}
