using FluentValidation;
using HMS.Models.Entities;
using HMS.Models.Enums;

namespace HMS.Models.DTOs.Patient
{
    public class CreatePatientRequestModel
    {
        public DateTime DateOfBirth { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }
        public string MedicalRecordNumber { get; set; }
        public string MedicalHistory { get; set; }
        public string BloodGroup { get; set; }
        public string Allergies { get; set; }
        public string EmergencyContact { get; set; }
        public string Genotype { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = [];
        public List<Guid> RoleIds { get; set; } = [];
        
    }
}
