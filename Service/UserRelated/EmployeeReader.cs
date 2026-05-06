using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Service.UserRelated.Interface;

namespace Service.UserRelated
{
    public class EmployeeReadService : IEmployeeReader
    {
        private readonly IEmployeeProfileRepository _employeeRepo;
        private readonly IMapper _mapper;

        public EmployeeReadService(IEmployeeProfileRepository employeeRepo, IMapper mapper)
        {
            _employeeRepo = employeeRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<IntranetEmployeeReadDto>> GetAllForIndex()
        {
            var employees = await _employeeRepo.GetAllAsync(null, false, e => e.ApplicationUser, e => e.Specializations);
            return _mapper.Map<IEnumerable<EmployeeProfile>, IEnumerable<IntranetEmployeeReadDto>>(employees);
        }
    }
}
