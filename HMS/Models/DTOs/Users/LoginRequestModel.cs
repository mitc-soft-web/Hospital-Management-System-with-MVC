using HMS.Models.DTOs.Doctor;
using HMS.Models.DTOs.Patients;
using HMS.Models.DTOs.Role;
using HMS.Models.Entities;

namespace HMS.Models.DTOs.Users
{
    public class LoginRequestModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponseModel : BaseResponse
    {
        public string FirstName { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public IEnumerable<RoleDto> Roles { get; set; } = new List<RoleDto>();

        public PatientDto Patient { get; set; }
        public DoctorDto Doctor { get; set; }
        public AdminDto Admin { get; set; }
    }
}
