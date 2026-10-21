using System.ComponentModel.DataAnnotations;

namespace PayrollManagementSystem.DTOs.Absences
{
    public class AbsenceRuleFormDto
    {
        public int Id { get; set; }

        [Display(Name = "من (عدد أيام الغياب)")]
        [Required(ErrorMessage = "بداية المدى مطلوبة")]
        [Range(0, 31, ErrorMessage = "عدد الأيام يجب أن يكون بين 0 و 31")]
        public int? FromDays { get; set; }

        // Empty = no upper limit (e.g. "more than 15 days")
        [Display(Name = "إلى")]
        [Range(0, 31, ErrorMessage = "عدد الأيام يجب أن يكون بين 0 و 31")]
        public int? ToDays { get; set; }

        [Display(Name = "النوع")]
        [Required(ErrorMessage = "النوع مطلوب")]
        public int? AbsenceRuleTypeId { get; set; }

        [Display(Name = "النسبة (%)")]
        [Required(ErrorMessage = "النسبة مطلوبة")]
        [Range(0.01,100, ErrorMessage = "النسبة يجب أن تكون أكبر من 0 ولا تزيد عن 100")]
        public decimal? Percentage { get; set; }
    }
}
