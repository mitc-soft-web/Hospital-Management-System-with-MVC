using HMS.Contracts.Entities;

namespace HMS.Models.Entities
{
    public class DoctorSpeciality : BaseEntity
    {
        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public Guid SpecialityId { get; set; }
        public Speciality Speciality { get; set; }
    }
}
