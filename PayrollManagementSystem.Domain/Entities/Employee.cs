namespace PayrollManagementSystem.Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateOnly? BirthDate { get; set; }

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public DateOnly HireDate { get; set; }

        public bool IsDeleted { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; } 

        public int JobGradeId { get; set; }
        public JobGrade JobGrade { get; set; } 
    }
}
