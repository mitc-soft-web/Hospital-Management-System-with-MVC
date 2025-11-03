using HMS.Models.Entities;
using HMS.Models.Enums;

namespace HMS.Models.DTOs.Doctor
{
    public class DoctorDto
    {
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string FirstName { get; set; }
        public string FullName {  get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int YearsOfExperience { get; set; }
        public string Qualification { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public ICollection<DoctorSpeciality> DoctorSpecialities { get; set; } = [];
        public ICollection<Entities.Appointment> Appointments { get; set; } = [];
    }
}
