using System.ComponentModel.DataAnnotations;

namespace PayrollManagementSystem.DTOs.Salaries
{
    public class SalaryFormDto
    {
        [Display(Name = "الدرجة الوظيفية")]
        [Required(ErrorMessage = "الدرجة الوظيفية مطلوبة")]
        public int? JobGradeId { get; set; }

        // Shown on the edit page only (the grade can't be changed there)
        public string? JobGradeName { get; set; }

        [Display(Name = "المرتب الأساسي")]
        [Required(ErrorMessage = "المرتب الأساسي مطلوب")]
        [Range(1, 9999999999999999.99, ErrorMessage = "المرتب يجب أن يكون أكبر من صفر")]
        public decimal? BaseSalary { get; set; }
    }
}
