using HMS.Models.Entities;

namespace HMS.Interfaces
{
    public interface IDoctorRepository : IBaseRepository
    {
        public Task<List<Doctor>> GetDoctorsBySpeciality(string  specialityName);
        public Task<IReadOnlyList<Doctor>> GetAllDoctorsAndTheirSpecialities();
    }
}
