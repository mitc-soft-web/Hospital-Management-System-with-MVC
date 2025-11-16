using HMS.Models.DTOs.Doctor;
using HMS.Models.DTOs.Patients;
using HMS.Models.Enums;

namespace HMS.Models.DTOs.Appointment
{
    public class AppointmentDto
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public Guid PatientId { get; set; }
        public PatientDto Patient { get; set; }
        public Guid DoctorId { get; set; }
        public DoctorDto Doctor { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string? ChangeInAppointmentDescription { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; }

        public DateTime AppointmentDate { get; set; }
    }
}
