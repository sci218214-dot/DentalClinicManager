using System;
using System.ComponentModel.DataAnnotations;

namespace DentalClinicManager.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }

        [Required(ErrorMessage = "تاريخ الموعد مطلوب")]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }   // ✅ مهم

        [Required(ErrorMessage = "وقت الموعد مطلوب")]
        [RegularExpression(@"^(?:[01]\d|2[0-3]):[0-5]\d$", ErrorMessage = "صيغة الوقت HH:mm مثل 09:30")]
        public string Time { get; set; } = string.Empty;

        [Range(5, 240, ErrorMessage = "مدة الموعد بين 5 و 240 دقيقة")]
        public int DurationMinutes { get; set; } = 30;

        [StringLength(500, ErrorMessage = "الملاحظات لا تتجاوز 500 حرف")]
        public string? Notes { get; set; }

        [Required(ErrorMessage = "المريض مطلوب")]
        [Range(1, int.MaxValue, ErrorMessage = "رقم المريض غير صحيح")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;

        [Required(ErrorMessage = "الطبيب مطلوب")]
        [Range(1, int.MaxValue, ErrorMessage = "رقم الطبيب غير صحيح")]
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; } = null!;
    }
}
