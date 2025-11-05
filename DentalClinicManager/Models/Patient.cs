using System;
using System.ComponentModel.DataAnnotations;

namespace DentalClinicManager.Models
{
    public class Patient
    {
        public int PatientId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; } = string.Empty;

        [Phone]
        public string Phone { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;
    }
}
