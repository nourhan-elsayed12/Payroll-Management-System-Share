using System.ComponentModel.DataAnnotations;

namespace PayrollManagementSystem.DTOs.Departments
{
    public class DepartmentFormDto
    {
        public int Id { get; set; }

        [Display(Name = "اسم القسم")]
        [Required(ErrorMessage = "اسم القسم مطلوب")]

        [StringLength(100, ErrorMessage = "اسم القسم يجب ألا يزيد عن 100 حرف")]
        public string Name { get; set; } 
    }
}
