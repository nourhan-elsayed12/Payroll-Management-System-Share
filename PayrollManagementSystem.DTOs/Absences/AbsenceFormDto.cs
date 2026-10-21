using System.ComponentModel.DataAnnotations;

namespace PayrollManagementSystem.DTOs.Absences
{
    public class AbsenceFormDto
    {
        // Form-only: fills the employees list, not saved with the absence
        [Display(Name = "القسم")]
        [Required(ErrorMessage = "القسم مطلوب")]
        public int? DepartmentId { get; set; }

        [Display(Name = "الموظف")]
        [Required(ErrorMessage = "الموظف مطلوب")]
        public int? EmployeeId { get; set; }

        [Display(Name = "تاريخ الغياب")]
        [Required(ErrorMessage = "تاريخ الغياب مطلوب")]
        [DataType(DataType.Date)]
        public DateOnly? Date { get; set; }
    }
}
