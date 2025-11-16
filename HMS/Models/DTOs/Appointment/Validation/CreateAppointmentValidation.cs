using FluentValidation;

namespace HMS.Models.DTOs.Appointment.Validation
{
    public class CreateAppointmentValidation : AbstractValidator<CreateAppointmentRequestModel>
    {
        public CreateAppointmentValidation()
        {
            RuleFor(x => x.DoctorId).NotEmpty().WithMessage("Doctor is required");
            RuleFor(x => x.PatientId).NotEmpty().WithMessage("Patient is required");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
            RuleFor(x => x.AppointmentDate).NotEmpty().WithMessage("Appointment date is required");
            RuleFor(x => x.Location).NotEmpty().WithMessage("Location is required");
           
        }
    }
}
