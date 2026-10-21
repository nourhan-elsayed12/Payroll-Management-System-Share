using System.ComponentModel.DataAnnotations;

namespace PayrollManagementSystem.DTOs.Incentives
{
    public class DepartmentIncentiveFormDto
    {
        [Display(Name = "القسم")]
        [Required(ErrorMessage = "القسم مطلوب")]
        public int? DepartmentId { get; set; }

        // Shown on the edit page only (the department can't be changed there)
        public string? DepartmentName { get; set; }

        [Display(Name = "نسبة الحافز (%)")]
        [Required(ErrorMessage = "نسبة الحافز مطلوبة")]
        [Range(0.01,100, ErrorMessage = "نسبة الحافز يجب أن تكون أكبر من 0 ولا تزيد عن 100")]
        public decimal? Percentage { get; set; }
    }
}
