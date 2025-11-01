using HMS.Models.Entities;

namespace HMS.Interfaces.Repositories
{
    public interface IPatientRepository : IBaseRepository
    {
        Task<Patient> Add(Patient entity);
    }
}
