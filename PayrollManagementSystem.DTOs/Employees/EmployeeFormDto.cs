using System.ComponentModel.DataAnnotations;

namespace PayrollManagementSystem.DTOs.Employees
{
    public class EmployeeFormDto
    {
        public int Id { get; set; }

        [Display(Name = "اسم الموظف")]
        [Required(ErrorMessage = "اسم الموظف مطلوب")]
        [StringLength(100, ErrorMessage = "اسم الموظف يجب ألا يزيد عن 100 حرف")]
        [RegularExpression(@"^[^0-9\u0660-\u0669\u06F0-\u06F9]*$", ErrorMessage = "اسم الموظف يجب ألا يحتوي على أرقام")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "تاريخ الميلاد")]
        [DataType(DataType.Date)]
        public DateOnly? BirthDate { get; set; }

        [Display(Name = "العنوان")]
        [StringLength(250, ErrorMessage = "العنوان يجب ألا يزيد عن 250 حرف")]
        public string? Address { get; set; }

        [Display(Name = "رقم الهاتف")]
        [RegularExpression(@"^01[0125][0-9]{8}$", ErrorMessage = "رقم الهاتف يجب أن يكون رقم موبايل مصري من 11 رقم يبدأ بـ 010 أو 011 أو 012 أو 015")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "البريد الإلكتروني")]
        [StringLength(150, ErrorMessage = "البريد الإلكتروني يجب ألا يزيد عن 150 حرف")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "البريد الإلكتروني غير صحيح")]
        public string? Email { get; set; }

        [Display(Name = "تاريخ التعيين")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "تاريخ التعيين مطلوب")]
        public DateOnly? HireDate { get; set; }

        [Display(Name = "القسم")]
        [Required(ErrorMessage = "القسم مطلوب")]
        public int? DepartmentId { get; set; }

        [Display(Name = "الدرجة الوظيفية")]
        [Required(ErrorMessage = "الدرجة الوظيفية مطلوبة")]
        public int? JobGradeId { get; set; }
    }
}
