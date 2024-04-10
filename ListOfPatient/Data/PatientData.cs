using ListingPatient.Models;
using System.ComponentModel.DataAnnotations;

namespace ListingPatient.Data
{
    public class PatientData
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public string IdentificationNumber { get; set; }
        [Required]
        public DateTime AddedTime { get; set; } = DateTime.Now;
        [Required]
        public string MedicalHistory { get; set; }
        public PatientStatus Status { get; set; } = PatientStatus.ChuaKham;
    }
}
