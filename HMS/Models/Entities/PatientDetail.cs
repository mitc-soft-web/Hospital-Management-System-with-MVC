namespace HMS.Models.Entities
{
    public class PatientDetail : BaseEntity
    {
        public string BloodGroup { get; set; }
        public string Allergies { get; set; }
        public string EmergencyContact { get; set; }
        public string Genotype { get; set; }
        public string MedicalHistory { get; set; }

        public Guid PatientId { get; set; }
    }
}
