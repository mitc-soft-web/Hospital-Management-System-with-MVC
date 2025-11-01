using HMS.Models.Entities;
using HMS.Models.Enums;

namespace HMS.Models.DTOs.Patients
{
    public class PatientDto
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string MedicalRecordNumber { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }

        public ICollection<Entities.Appointment> Appointments { get; set; } = [];
        public PatientDetail PatientDetail { get; set; }
    }
}
