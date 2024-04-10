namespace ListingPatient.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string IdentificationNumber { get; set; } 
        public PatientStatus Status { get; set; }
        public DateTime AddedTime { get; set; } = DateTime.Now;
        public string MedicalHistory { get; set; }
    }
}
