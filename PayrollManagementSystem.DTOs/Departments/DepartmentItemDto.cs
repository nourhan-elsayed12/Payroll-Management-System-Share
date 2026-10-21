using System;
using System.Collections.Generic;
using System.Text;

namespace PayrollManagementSystem.DTOs.Departments
{
    public class DepartmentItemDto
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public int EmployeesCount { get; init; }
    }
}
