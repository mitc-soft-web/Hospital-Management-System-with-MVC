using HMS.Models.Entities;
using HMS.Models.Enums;

namespace HMS.Models.DTOs.Specialty
{
    public class SpecialtyDto
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<DoctorSpeciality> DoctorSpecialities { get; set; } = [];
    }
}
