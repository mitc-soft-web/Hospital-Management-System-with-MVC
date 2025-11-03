using HMS.Models.DTOs.Role;

namespace HMS.Models.DTOs.Users
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }

        public string Email { get; set; }
        public ICollection<RoleDto> UserRoles { get; set; } = [];

    }
}
