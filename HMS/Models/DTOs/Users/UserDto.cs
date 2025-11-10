using HMS.Models.DTOs.Doctor;
using HMS.Models.DTOs.Patients;
using HMS.Models.DTOs.Role;

namespace HMS.Models.DTOs.Users
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }

        public string Email { get; set; }
        public ICollection<RoleDto> Roles { get; set; } = [];
        public PatientDto Patient { get; set; }
        public DoctorDto Doctor { get; set; }
        public AdminDto Admin { get; set; }

    }
}
