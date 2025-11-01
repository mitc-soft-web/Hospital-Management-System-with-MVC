using HMS.Contracts.Entities;
using HMS.Models.Enums;

namespace HMS.Models.Entities
{
    public class Patient : BaseUser
    {
        public string MedicalRecordNumber { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = [];
        public PatientDetail PatientDetail { get; set; }
    }
}
