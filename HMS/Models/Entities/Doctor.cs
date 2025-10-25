using HMS.Models.Enums;

namespace HMS.Models.Entities
{
    public class Doctor : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int YearsOfExperience { get; set; }
        public string Qualification { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public ICollection<DoctorSpeciality> DoctorSpecialities { get; set; } = [];
        public ICollection<Appointment> Appointments { get; set; } = [];

    }
}
