using FluentValidation;

namespace HMS.Models.DTOs.Patient.Validation
{
    public class CreatePatientValidation : AbstractValidator<CreatePatientRequestModel>
    {
        public CreatePatientValidation()
        {
            RuleFor(x => x.FirstName).Length(3, 50).NotEmpty().WithMessage("Firstname is required");
            RuleFor(x => x.LastName).Length(3, 50).NotEmpty().WithMessage("Lastname is required");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
            RuleFor(x => x.PasswordHash).NotEmpty().WithMessage("Password is required");
            RuleFor(x => x.ConfirmPassword).Matches(x => x.PasswordHash).NotEmpty().WithMessage("Confirm password is required");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required");
            RuleFor(x => x.DateOfBirth).NotEmpty().WithMessage("Date of birth is required");
            RuleFor(x => x.Gender).NotEmpty().WithMessage("Gender is required");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number is required");
            RuleFor(x => x.BloodGroup).NotEmpty().WithMessage("Blood group is required");
            RuleFor(x => x.Genotype).NotEmpty().WithMessage("Genotype is required");
            RuleFor(x => x.EmergencyContact).NotEmpty().WithMessage("Emergency contact is required");

        }
    }
}
