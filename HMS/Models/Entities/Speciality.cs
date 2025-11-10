using HMS.Contracts.Entities;
using HMS.Models.Enums;

namespace HMS.Models.Entities
{
    public class Speciality : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }

        public ICollection<DoctorSpeciality> DoctorSpecialities { get; set; } = [];


    }
}
