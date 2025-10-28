using HMS.Models.Contracts;

namespace HMS.Models.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }

        public Guid? PatientId {  get; set; }

        public Patient? Patient { get; set; }
        public Guid DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
        public Guid AdminId { get; set; }
        public Admin? Admin { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = [];


        public string ChangePassword(string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
            {
                throw new ArgumentException("Password cannot be empty", nameof(newPassword));
            }
            PasswordHash = newPassword;
            return PasswordHash;
        }
    }
}
