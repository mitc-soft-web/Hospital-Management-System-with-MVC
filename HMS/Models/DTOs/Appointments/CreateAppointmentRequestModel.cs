using HMS.Models.Enums;

namespace HMS.Models.DTOs.Appointment
{
    public class CreateAppointmentRequestModel
    {
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
