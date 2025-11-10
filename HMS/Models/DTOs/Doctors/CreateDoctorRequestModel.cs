using HMS.Models.Entities;
using HMS.Models.Enums;

namespace HMS.Models.DTOs.Doctor
{
    public class CreateDoctorRequestModel
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string ConfirmPassword { get; set; }

        public DateTime DateOfBirth { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Position Position { get; set; }
        public Gender Gender { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int YearsOfExperience { get; set; }
        public string Qualification { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public IList<Guid> SpecialityIds { get; set; } = [];
        public List<Guid> RoleIds { get; set; } = [];
    }
}
