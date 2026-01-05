using System;
using System.ComponentModel.DataAnnotations;

namespace DentalClinicManager.Models
{
    public class Patient
    {
        public int PatientId { get; set; }

        [Required(ErrorMessage = "الاسم مطلوب")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "الاسم يجب أن يكون بين 3 و 100 حرف")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "تاريخ الميلاد مطلوب")]
        [DataType(DataType.Date)]
        [Display(Name = "تاريخ الميلاد")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "الجنس مطلوب")]
        [StringLength(10, ErrorMessage = "قيمة الجنس غير صحيحة")]
        public string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        [StringLength(20, ErrorMessage = "رقم الهاتف طويل جدًا")]
        public string Phone { get; set; } = string.Empty;

        // خليته اختياري لأن بعض المرضى ما عندهم بريد
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        [StringLength(100, ErrorMessage = "البريد الإلكتروني طويل جدًا")]
        public string? Email { get; set; }

        [StringLength(200, ErrorMessage = "العنوان لا يتجاوز 200 حرف")]
        public string Address { get; set; } = string.Empty;
    }
}
