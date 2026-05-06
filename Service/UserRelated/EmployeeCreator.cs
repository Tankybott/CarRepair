using AutoMapper;
using DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Service.UserRelated.Interface;

namespace Service.UserRelated
{
    public class EmployeeCreator : IEmployeeCreator
    {
        private readonly IEmployeeProfileRepository _employeeRepo;
        private readonly IServiceTypeRepository _serviceTypeRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeCreator> _logger;

        public EmployeeCreator(IEmployeeProfileRepository employeeRepo, IServiceTypeRepository serviceTypeRepo, UserManager<ApplicationUser> userManager, IMapper mapper, ILogger<EmployeeCreator> logger)
        {
            _employeeRepo = employeeRepo;
            _serviceTypeRepo = serviceTypeRepo;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IntranetEmployeeReadDto> CreateAsync(IntranetEmployeeCreateDto dto)
        {
            var user = new ApplicationUser { UserName = dto.Email, Email = dto.Email };
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                _logger.LogError("Failed to create user for employee: {Errors}", errors);
                throw new InvalidOperationException(errors);
            }

            var profile = new EmployeeProfile
            {
                ApplicationUserId = user.Id,
                Name = dto.Name,
                Surname = dto.Surname,
                EmployeeNumber = dto.EmployeeNumber
            };

            if (dto.SpecializationIds.Any())
            {
                var specs = await _serviceTypeRepo.GetAllAsync(st => dto.SpecializationIds.Contains(st.Id), true);
                foreach (var spec in specs)
                    profile.Specializations.Add(spec);
            }

            _employeeRepo.Add(profile);
            await _employeeRepo.SaveAsync();

            var created = await _employeeRepo.GetAsync(e => e.Id == profile.Id, false, e => e.ApplicationUser, e => e.Specializations);
            return _mapper.Map<IntranetEmployeeReadDto>(created!);
        }
    }
}
