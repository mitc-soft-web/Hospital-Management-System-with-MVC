using HMS.Contracts.Entities;
using HMS.Models.Enums;

namespace HMS.Models.Entities
{
    public class Doctor : BaseUser
    {
        
        public int YearsOfExperience { get; set; }
        public string Qualification { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public ICollection<DoctorSpeciality> DoctorSpecialities { get; set; } = [];
        public ICollection<Appointment> Appointments { get; set; } = [];

    }
}
