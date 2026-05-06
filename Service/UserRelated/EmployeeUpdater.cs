using AutoMapper;
using DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Service.UserRelated.Interface;

namespace Service.UserRelated
{
    public class EmployeeUpdater : IEmployeeUpdater
    {
        private readonly IEmployeeProfileRepository _employeeRepo;
        private readonly IServiceTypeRepository _serviceTypeRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeUpdater> _logger;

        public EmployeeUpdater(IEmployeeProfileRepository employeeRepo, IServiceTypeRepository serviceTypeRepo, UserManager<ApplicationUser> userManager, IMapper mapper, ILogger<EmployeeUpdater> logger)
        {
            _employeeRepo = employeeRepo;
            _serviceTypeRepo = serviceTypeRepo;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IntranetEmployeeReadDto> UpdateAsync(IntranetEmployeeUpdateDto dto)
        {
            var profile = await _employeeRepo.GetAsync(e => e.Id == dto.Id, true, e => e.ApplicationUser, e => e.Specializations);

            if (profile == null)
                throw new Exception($"Employee with id {dto.Id} not found.");

            profile.Name = dto.Name;
            profile.Surname = dto.Surname;
            profile.EmployeeNumber = dto.EmployeeNumber;

            profile.Specializations.Clear();
            if (dto.SpecializationIds.Any())
            {
                var newSpecs = await _serviceTypeRepo.GetAllAsync(st => dto.SpecializationIds.Contains(st.Id), true);
                foreach (var spec in newSpecs)
                    profile.Specializations.Add(spec);
            }

            _employeeRepo.Update(profile);
            await _employeeRepo.SaveAsync();

            if (!string.IsNullOrWhiteSpace(dto.NewPassword))
            {
                var user = profile.ApplicationUser ?? await _userManager.FindByIdAsync(profile.ApplicationUserId);

                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, token, dto.NewPassword);

                    if (!result.Succeeded)
                    {
                        var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                        _logger.LogError("Failed to reset password for employee {Id}: {Errors}", dto.Id, errors);
                        throw new InvalidOperationException(errors);
                    }
                }
            }

            var updated = await _employeeRepo.GetAsync(e => e.Id == profile.Id, false, e => e.ApplicationUser, e => e.Specializations);
            return _mapper.Map<IntranetEmployeeReadDto>(updated!);
        }
    }
}
