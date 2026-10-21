namespace PayrollManagementSystem.Domain.Entities
{
    public class Department
    {
        public int Id { get; set; }

        public string Name { get; set; } 

        public decimal IncentivePercentage { get; set; }

        public ICollection<Employee> Employees { get; set; } 
    }
}
