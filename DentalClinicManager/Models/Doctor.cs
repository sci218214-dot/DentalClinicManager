using System.ComponentModel.DataAnnotations;

namespace DentalClinicManager.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "اسم الطبيب مطلوب")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "اسم الطبيب يجب أن يكون بين 3 و 100 حرف")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "التخصص مطلوب")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "التخصص يجب أن يكون بين 2 و 100 حرف")]
        public string Specialty { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        [StringLength(20, ErrorMessage = "رقم الهاتف طويل جدًا")]
        public string Phone { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        [StringLength(100, ErrorMessage = "البريد الإلكتروني طويل جدًا")]
        public string? Email { get; set; }
    }
}
