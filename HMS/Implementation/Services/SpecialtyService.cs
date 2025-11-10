using HMS.Interfaces.Repositories;
using HMS.Interfaces.Services;
using HMS.Models.DTOs;
using HMS.Models.DTOs.Specialty;
using HMS.Models.Entities;

namespace HMS.Implementation.Services
{
    public class SpecialtyService : ISpecialtyService
    {
        private readonly ISpecialityRepository _specialityRepository;
        private readonly ILogger<SpecialtyService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public SpecialtyService(ISpecialityRepository specialityRepository,
            IUnitOfWork unitOfWork,
            ILogger<SpecialtyService> logger)
        {
            _logger = logger;
            _specialityRepository = specialityRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<BaseResponse<bool>> CreateAsync(CreateSpecialtyRequestModel request)
        {
            var specialtyExists = await _specialityRepository.Any(s => s.Name == request.Name);
            if (specialtyExists)
            {
                return new BaseResponse<bool>
                {
                    Message = "Speciality already exists",
                    Status = false
                };
            }
            var speciality = new Speciality
            {
                Name = request.Name,
                Description = request.Description,
                DateCreated = DateTime.UtcNow,
            };

            var createSpecialty = await _specialityRepository.Add(speciality);
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);

            if (createSpecialty != null)
            {
                return new BaseResponse<bool>
                {
                    Message = "Specialty couldn't be created",
                    Status = false,

                };

            }
            return new BaseResponse<bool>
            {
                Message = "Specialty created successfully",
                Status = true
            };

        }

        public Task<BaseResponse<bool>> DeleteAsync(Guid specialtyId)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<SpecialtyDto>> GetAsync(string param, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<SpecialtyDto>> GetByIdAsync(Guid specialtyId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<IEnumerable<SpecialtyDto>>> GetSpecialitiesAsync(CancellationToken cancellationToken)
        {
            var specialities = await _specialityRepository.GetAll<Speciality>();
            if (!specialities.Any())
            {
                return new BaseResponse<IEnumerable<SpecialtyDto>>
                {
                    Message = "No data found",
                    Status = false
                };
            }

            return new BaseResponse<IEnumerable<SpecialtyDto>>
            {
                Message = "Data fetched successfully",
                Status = true,
                Data = specialities.Select(s => new SpecialtyDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    DateCreated = s.DateCreated,
                }).ToList()
            };
        }
    }
}
