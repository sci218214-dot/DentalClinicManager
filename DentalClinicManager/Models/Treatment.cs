using System;
using System.ComponentModel.DataAnnotations;

namespace DentalClinicManager.Models
{
    public class Treatment
    {
        public int TreatmentId { get; set; }

        [Required(ErrorMessage = "وصف العلاج مطلوب")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "وصف العلاج يجب أن يكون بين 5 و 500 حرف")]
        public string Description { get; set; } = string.Empty;

        [Range(0, 1000000, ErrorMessage = "التكلفة يجب أن تكون بين 0 و 1,000,000")]
        [DataType(DataType.Currency)]
        public decimal Cost { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "تاريخ العلاج")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "المريض مطلوب")]
        [Range(1, int.MaxValue, ErrorMessage = "رقم المريض غير صحيح")]
        public int PatientId { get; set; }

        public Patient Patient { get; set; } = null!;

        // إذا العلاج لازم يكون مرتبط بطبيب خله Required وخلي Doctor غير nullable
        [Required(ErrorMessage = "الطبيب مطلوب")]
        [Range(1, int.MaxValue, ErrorMessage = "رقم الطبيب غير صحيح")]
        public int DoctorId { get; set; }

        public Doctor Doctor { get; set; } = null!;
    }
}
