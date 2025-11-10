using HMS.Models.Entities;

namespace HMS.Interfaces.Repositories
{
    public interface IDoctorRepository : IBaseRepository
    {
        public Task<List<Doctor>> GetDoctorsBySpeciality(string specialityName);
        public Task<IReadOnlyList<Doctor>> GetAllDoctorsAndTheirSpecialities();
        public Task<Doctor> GetDoctorByIdAsync(Guid doctorId);
        Task<int> GetDoctorCounts();
    }
}
