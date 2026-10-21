namespace PayrollManagementSystem.Domain.Entities
{
    /// <summary>One absence day of an employee.</summary>
    public class Absence
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public Employee Employee { get; set; } = null!;

        public DateOnly Date { get; set; }
    }
}
