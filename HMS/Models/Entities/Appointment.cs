using HMS.Models.Contracts;
using HMS.Models.Enums;

namespace HMS.Models.Entities
{
    public class Appointment : BaseEntity
    {
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }
        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string ChangeInAppointmentDescription { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; }

        public DateTime AppointmentDate { get; set; }
    }
}
