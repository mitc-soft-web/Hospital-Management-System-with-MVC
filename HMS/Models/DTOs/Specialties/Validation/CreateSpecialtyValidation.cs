using FluentValidation;

namespace HMS.Models.DTOs.Specialty.Validation
{
    public class CreateSpecialtyValidation : AbstractValidator<CreateSpecialtyRequestModel>
    {
        public CreateSpecialtyValidation()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");

        }
    }
}
