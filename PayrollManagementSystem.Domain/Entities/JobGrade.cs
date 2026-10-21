namespace PayrollManagementSystem.Domain.Entities
{
    public class JobGrade
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal BaseSalary { get; set; }

        public ICollection<Employee> Employees { get; set; } 
    }
}
