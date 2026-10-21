namespace PayrollManagementSystem.DTOs.Reports
{
    public class IncentivesReportRowDto
    {
        public string EmployeeName { get; init; } = string.Empty;

        // Base salary of the employee's job grade (shown in the salaries report)
        public decimal BaseSalary { get; init; }

        public decimal AbsenceDeduction { get; init; }

        public decimal DepartmentIncentive { get; init; }

        public decimal ServiceYearsIncentive { get; init; }

        public decimal AbsenceIncentive { get; init; }

        public decimal TotalIncentives => DepartmentIncentive + ServiceYearsIncentive + AbsenceIncentive;

        public decimal NetSalary => BaseSalary + TotalIncentives - AbsenceDeduction;
    }
}
