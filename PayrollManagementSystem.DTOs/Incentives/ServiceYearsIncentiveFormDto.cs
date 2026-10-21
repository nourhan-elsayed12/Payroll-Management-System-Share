using System.ComponentModel.DataAnnotations;

namespace PayrollManagementSystem.DTOs.Incentives
{
    public class ServiceYearsIncentiveFormDto
    {
        public int Id { get; set; }

        [Display(Name = "عدد سنوات الخدمة (أكثر من)")]
        [Required(ErrorMessage = "عدد سنوات الخدمة مطلوب")]
        [Range(1, 50, ErrorMessage = "عدد سنوات الخدمة يجب أن يكون بين 1 و 50")]
        public int? MinYears { get; set; }

        [Display(Name = "نسبة الحافز (%)")]
        [Required(ErrorMessage = "نسبة الحافز مطلوبة")]
        [Range(typeof(decimal), "0.01", "100", ParseLimitsInInvariantCulture = true, ErrorMessage = "نسبة الحافز يجب أن تكون أكبر من 0 ولا تزيد عن 100")]
        public decimal? Percentage { get; set; }
    }
}
