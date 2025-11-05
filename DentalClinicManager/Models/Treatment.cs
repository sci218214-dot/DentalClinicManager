using System;

namespace DentalClinicManager.Models
{
    public class Treatment
    {
        public int TreatmentId { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public DateTime Date { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;

        public int DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
    }
}
